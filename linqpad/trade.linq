<Query Kind="SQL">
  <Connection>
    <ID>a093c89a-b903-4883-b955-f2eeed2e9931</ID>
    <Driver Assembly="IQDriver" PublicKeyToken="5b59726538a49684">IQDriver.IQDriver</Driver>
    <Provider>Devart.Data.MySql</Provider>
    <CustomCxString>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA2mAEOhngkEiIh8AlPkfRqwAAAAACAAAAAAAQZgAAAAEAACAAAADC4+i9gLrETcMzG+m1zhiSWCd+W2XgXR4Tah09YhmRcwAAAAAOgAAAAAIAACAAAABkwywlYCbLm4Za5KWSd+pV/vYhcTPcTp0S/X+3BPAXz1AAAAA+U1FAf3YJSA5wxHO59+sn8ddS0KnNs8HxEDoc2mKvgxmzHRfL6K4M8U0pgiR1tpbNBqS+65yc4LJ3iqgx1LfwRQYKc8cxSgDisJhC/t7Md0AAAADeX6iKIHVjsFmR7yZlueQMODHVKKeBTgVnoonny/deyrEmVlsiECqWpjgRRwBPtlYN11gHUoAgB2X58tNZjh6t</CustomCxString>
    <NoPluralization>true</NoPluralization>
    <Server>127.0.0.1</Server>
    <Database>store</Database>
    <UserName>root</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA2mAEOhngkEiIh8AlPkfRqwAAAAACAAAAAAAQZgAAAAEAACAAAABQJoBhEfCLNH67sVvEpKLv4xfulmsrVx41l3fKbHXZwgAAAAAOgAAAAAIAACAAAAD/K/CVJqnKehc2p6B2bRahTiBp87fEI1Udqd1IeSiwjBAAAACX+J90U8/Zm6ZyUZMScKVeQAAAAC1StlosNAd5TnsT3uOKgiW8IX6nC70eZOx91Gg3bJhV29W9vqjH4gaBLOf0QYeCBKhfjaqmKwtFfn041lBQrQM=</Password>
    <EncryptCustomCxString>true</EncryptCustomCxString>
    <NoCapitalization>true</NoCapitalization>
    <DriverData>
      <StripUnderscores>false</StripUnderscores>
      <QuietenAllCaps>false</QuietenAllCaps>
      <Port>3306</Port>
      <ExtraCxOptions>charset=utf8</ExtraCxOptions>
    </DriverData>
  </Connection>
</Query>

/*
drop database if exists Trade;

create database Trade;
*/

use Trade;

drop table if exists `Order`;
create table `Order`(
Id char(18) not null,
GroupId tinyint default 0 not null,/*订单类别:0网购订单 1竞购订单 2竞购补差订单 3积分兑换订单*/
Quantity int default 0 not null,/*商品总数量*/
Total decimal(8,2) default 0 not null,/*订单总金额(需要支付的总金额[SubTotal+Postage-Discount])*/
SubTotal decimal(8,2) default 0 not null,/*订单小计(商品总金额)*/
Postage int default 0 not null,/*运费*/
Discount decimal(8,2) default 0 not null,/*订单总折扣*/
Paid decimal(8,2) default 0 not null,/*已支付金额(一个订单可能分多次支付)*/
IntegralUse int default 0 not null,/*使用积分*/
IntegralRealUse int default 0 not null,
IntegralReward int default 0 not null,
Coupon varchar(36) default 0,/*优惠券*/
CouponUse decimal(8,2) default 0 not null,/*优惠券抵消金额*/
GiftCard varchar(36) default 0, /*礼品卡*/
GiftCardUse decimal(8,2) default 0 not null,/*礼品卡抵消金额*/

Weight decimal(8,2) default 0 not null,/*重量*/
ETicket varchar(200),

PaymentType tinyint default 0 not null,/*支付方式*/
ShipmentType tinyint default 0 not null,/*配送方式*/
MultiConsigned bool default 0 not null,/*是否分拆成多个包裹发货*/

PaidOn datetime,/*支付日期(最后一次支付完成整个订单的时间)*/
ConsignedOn datetime,/*发货日期*/
FinishedOn datetime,/*结束日期 交易成功时间(更新交易状态为成功的同时更新)/确认收货时间或者交易关闭时间 */
ExpiredOn datetime not null,/*超时时间*/

Consignee nvarchar(50),
Phone varchar(25),
Mobile varchar(11),
RegionId varchar(6),
Province varchar(50),
City varchar(50),
District varchar(50),
Address nvarchar(200),
PostalCode varchar(20),

HasInvoice bit default 0 not null,
InvoiceGroup nvarchar(50),
InvoiceTitle nvarchar(100),
InvoiceContent nvarchar(100),

BuyerId int not null,
BuyerName nvarchar(20),
BuyerFlag tinyint not null,
BuyerMemo nvarchar(100),
BuyerMessage nvarchar(500),
BuyerRate bit not null,
SellerId int not null,
SellerName nvarchar(20),
SellerFlag tinyint not null,
SellerMemo nvarchar(100),
SellerRate bit not null,

CancelId tinyint default 0,
CancelReason nvarchar(200),
Exchange tinyint default 0 not null,/*兑换 0为None 1为兑换为积分 2为兑换为免费拍币*/
AssociateId varchar(50) not null,
Extra nvarchar(200),
Status tinyint not null,
MediaId int default 0 not null,
IpAddress varchar(50) not null,
TraceCode varchar(50) not null,/*订单跟踪编码*/
CreatedOn datetime default current_timestamp  not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单';


drop table if exists OrderItem;
create table OrderItem(
Id char(20) not null,
OrderId char(18) not null,
SkuId int default 0 not null,
ItemId int default 0 not null,
SpuId int default 0 not null,
CatgId int default 0 not null,/*最小类目id*/
CatgRId int default 0 not null,/*根类目id*/
CatgMId varchar(100) default '',/*中间类目id*/
OuterId varchar(100) default '',
Name nvarchar(100),
Price decimal(8,2) default 0 not null,/*单价*/
Quantity int default 0 not null,
Discount decimal(8,2) default 0 not null,/*折扣*/
Adjust decimal(8,2) default 0 not null,
Total decimal(8,2) default 0 not null,
MealId int default 0 not null,
MealName nvarchar(100),
Picture varchar(500),
PropText nvarchar(500),
BuyerId int not null,
SellerId int not null,

IsGift bool default 0 not null,
IsVirtual bool default 0 not null,
IsService bool default 0 not null,
ConsignedOn datetime,/*发货日期*/
FinishedOn datetime,/*结束日期 交易成功时间(更新交易状态为成功的同时更新)/确认收货时间或者交易关闭时间 */
ExpiredOn datetime not null,/*超时时间*/
ShipmentType tinyint default 0 not null,/*配送方式*/
LogisticsNo varchar(50),
LogisticsCompany nvarchar(50),
RefundId char(24),
RefundStatus tinyint,
Extra nvarchar(200),
Status tinyint default 0 not null,
CreatedOn datetime default current_timestamp  not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单';


drop table if exists OrderNote;
create table OrderNote(
Id int auto_increment not null,
GroupId tinyint default 0 not null,
OrderId char(18) not null,
Message nvarchar(200),
ForAdmin bool default 0 not null,
CreatedBy nvarchar(50) default '',
CreatedOn datetime default current_timestamp  not null,
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='订单';