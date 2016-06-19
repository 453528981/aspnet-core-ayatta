<Query Kind="SQL">
  <Connection>
    <ID>fef888ea-15b7-47c6-9238-a0deb1b806a7</ID>
    <Persist>true</Persist>
    <Driver Assembly="IQDriver" PublicKeyToken="5b59726538a49684">IQDriver.IQDriver</Driver>
    <Provider>Devart.Data.MySql</Provider>
    <CustomCxString>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAasoE5ACSEE6Rts5OBZSDZQAAAAACAAAAAAAQZgAAAAEAACAAAAAftQNo14Qo71gMV+70G2Qpoem1Xy0eSRZY+gHQ6ZNyjQAAAAAOgAAAAAIAACAAAAAUtgkLcE5PXsKYqQCWucb4NoRqU7s3jvv1wHmSb93EAVAAAAAYGGdbkR1tQ/Uyk3pTASXgcFKYchnhws8lWIK0BxHzIOCKnCDmbABrpF+CCtxLAuGJYEwlXhQACwNDw7DCPTOfPBeEl22j8aNVVbSagO+6Y0AAAAD10P9DkVD60KA+EQ0J6ICnFjcxWyv9oo8fnoidGQuMem4r5txCCujVKxCdLQ3fE3mAtSbkFGtDZrfFfpQeODz+</CustomCxString>
    <Server>127.0.0.1</Server>
    <Database>keyword</Database>
    <NoPluralization>true</NoPluralization>
    <NoCapitalization>true</NoCapitalization>
    <UserName>root</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAgeh4eyutP0SFuhsW6pKTLAAAAAACAAAAAAAQZgAAAAEAACAAAAAgaTcRNdhriXdroFz3e0hv72DZ0V2gLDzFLJ6UUOMYUAAAAAAOgAAAAAIAACAAAADSXQcm0DOrcYcO8aB4lFJmqTiFRnpkzuL0VX+og4gVCxAAAABhwTV+kk/af8270IBpDOzrQAAAALsJKf3PNd//bahw6xVo1e/pZXSpBVEaeJ60h6eGJ01OHhA5oH0R9dEdmxhyfnrOLkQ/ZnczH6538eiSBxkaYeA=</Password>
    <EncryptCustomCxString>true</EncryptCustomCxString>
    <DriverData>
      <StripUnderscores>false</StripUnderscores>
      <QuietenAllCaps>false</QuietenAllCaps>
      <ExtraCxOptions>charset=utf8</ExtraCxOptions>
    </DriverData>
  </Connection>
</Query>

/*
drop database if exists Store;

create database Store;
*/

use store;

drop table if exists Item;
create table Item(
Id int auto_increment not null comment 'Id',
SpuId int default 0 not null comment 'SpuId',
UserId int default 0 not null comment '商家Id',
CatgId int default 0 not null comment '最小类目id',
CatgRId int default 0 not null comment '根类目id',
CatgMId varchar(100) default '' comment '中间类目id',
Code varchar(100) default '' comment '商家设置的外部id',
Name nvarchar(80) not null comment '商品名称,不能超过60字节',
Title nvarchar(80) comment '标题',
Stock int default 0 not null comment '商品库存数量',
Price decimal(10,2) default 0 not null comment '商品价格',
AppPrice decimal(10,2) default 0 not null comment 'app商品价格',
RetailPrice decimal(10,2) default 0 not null comment '商品建议零售价格',
Barcode varchar(50) default '' comment '条形码',
BrandId int default 0 not null comment '品牌Id',
BrandName nvarchar(100) default '' comment '品牌名',
Keyword nvarchar(2000) comment '关键字',
PropId nvarchar(500) comment '商品属性Id 格式：pid:vid;pid:vid',
PropStr nvarchar(1000) comment '商品属性值 格式 pid:vid:pname:vname;pid:vid:pname:vname',
PropAlias nvarchar(500) comment '属性值别名,比如颜色的自定义名称 1627207:28335:草绿;1627207:3232479:深紫',
InputId nvarchar(500) comment '商品输入属性Id',
InputStr nvarchar(500) comment '商品输入属性值',
Width decimal(10,2) default 0 not null comment '宽度',
Depth decimal(10,2) default 0 not null comment '深度',
Height decimal(10,2) default 0 not null comment '高度',
Weight decimal(10,2) default 0 not null comment '重量',
Summary nvarchar(1000) comment '商品概要',
Picture nvarchar(500) comment '商品主图片地址',
ItemImgStr nvarchar(1000) comment '商品图片列表(包括主图)',
PropImgStr nvarchar(1000) comment '商品属性图片列表',
IsVirtual bool default 0 not null comment '是否为虚拟物品',
IsAutoFill bool default 0 not null comment '代充商品类型 可选类型： timecard(点卡软件代充) feecard(话费软件代充)',
IsTiming bool default 0 not null comment '是否定时上架商品',
SubStock tinyint default 0 not null comment '0为拍下减库存 1为付款减库存',
Showcase int default 0 not null comment '橱窗推荐',
OnlineOn datetime not null comment '上架时间',
OfflineOn datetime not null comment '下架时间',
RewardRate decimal(2,2) default 0.2 not null comment '积分奖励',
HasInvoice bool default 0 not null comment '是否有发票',
HasWarranty bool default 0 not null comment '是否有保修',
HasGuarantee bool default 0 not null comment '是否承诺退换货服务',
SaleCount int default 0 not null comment '销售数量',
CollectCount int default 0 not null comment '收藏数量',
ConsultCount int default 0 not null comment '咨询数量',
CommentCount int default 0 not null comment '评论数量',
Status tinyint default 0 not null comment '状态 0为可用',
CreatedOn datetime default current_timestamp  not null comment '创建时间',
ModifiedBy nvarchar(50) default '' comment '最后一次编辑者',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品';

drop table if exists ItemDesc;
create table ItemDesc(
Id int not null,
Useage text comment '使用方法',
Notice text comment '使用须知',
Detail text comment '商品详情',
AppDetail text comment '商品详情',
Photograph text comment '产品实拍',
CreatedOn datetime default current_timestamp  not null comment '创建时间',
ModifiedBy nvarchar(50) default '' comment '最后一次编辑者',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品描述';


drop table if exists Sku;
create table Sku(
Id int auto_increment not null,
SpuId int default 0 not null comment 'SpuId',
ItemId int default 0 not null comment 'ItemId',
UserId int default 0 not null comment '商家Id',
CatgId int default 0 not null  comment '最小类目id',
CatgRId int default 0 not null  comment '根类目id',
CatgMId varchar(100) default '' comment '中间类目id',
Code varchar(100) default '' comment '商家设置的外部id',
Barcode varchar(50) default '' comment '条形码',
BrandId int default 0 not null comment '品牌Id',
Stock int default 0 not null comment 'Sku库存数量',
Price decimal(10,2) default 0 not null comment 'Sku价格',
AppPrice decimal(10,2) default 0 not null comment 'Sku app 价格',
PropId nvarchar(500) comment 'Sku属性Id 格式：pid:vid;pid:vid',
PropStr nvarchar(1000) comment 'Sku属性值 格式 pid:vid:pname:vname;pid:vid:pname:vname',
SaleCount int default 0 not null comment '销售数量',
Status tinyint default 0 not null comment '状态 0为可用',
CreatedOn datetime default current_timestamp  not null comment '创建时间',
ModifiedBy nvarchar(50) default '' comment '最后一次编辑者',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品Sku';