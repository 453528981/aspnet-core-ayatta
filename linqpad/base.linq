<Query Kind="SQL">
  <Connection>
    <ID>577999c1-607c-4e5b-8d77-61164c3b7c34</ID>
    <Driver Assembly="IQDriver" PublicKeyToken="5b59726538a49684">IQDriver.IQDriver</Driver>
    <Provider>Devart.Data.MySql</Provider>
    <CustomCxString>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA2mAEOhngkEiIh8AlPkfRqwAAAAACAAAAAAAQZgAAAAEAACAAAAC3e/KYteKnGXO0eZDCLTDDqTGF/bUB9H/tiYnTwJtjYwAAAAAOgAAAAAIAACAAAAD92/y7J/yb0LbjZ+gGyFFPSnFDrBtq+I4rUPcERlvqy0AAAADfok9QWwteEOomO0Ghph+63e1pGCNS0fr0gc9agDkaewBoXyjAyrNw65bDKX13CjamscKyqyard9EuRYEe14jWQAAAADuIzLOi/0KFsLozcrheM2J580DXG/iab8XmKdzW1uO9IpSjmXklFkuhVNMvFpDSLkxGCcg0qmewgLaXvAr1ix8=</CustomCxString>
    <Server>127.0.0.1</Server>
    <Database>store</Database>
    <UserName>root</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA2mAEOhngkEiIh8AlPkfRqwAAAAACAAAAAAAQZgAAAAEAACAAAACCPlqo04bYfH8ww/fLZGt6lqv/3LmbUXBvrSXs3leprwAAAAAOgAAAAAIAACAAAACDI8lIJeLsPM0z6O7qAZIn2iMsSImOHtj1KBDUqbpS+RAAAADgMdW7YiI0MjnQOe+eD+AdQAAAALFPYobyGPr/gBUFDIR9OILfkt1XSfp0tTt2KK32GwRkzn3rK+6RJjQQIaZKtnlbwaUKwl2+/Xs0jX3yPAYa120=</Password>
    <NoCapitalization>true</NoCapitalization>
    <NoPluralization>true</NoPluralization>
    <EncryptCustomCxString>true</EncryptCustomCxString>
    <DriverData>
      <StripUnderscores>false</StripUnderscores>
      <QuietenAllCaps>false</QuietenAllCaps>
      <ExtraCxOptions>charset=utf8</ExtraCxOptions>
    </DriverData>
  </Connection>
</Query>

/*
drop database if exists Base;

create database Base;
*/

use base;

drop table if exists ProdCatg;
create table ProdCatg(
Id int auto_increment not null,
Name nvarchar(100) not null,
ParentId int default 0 not null,
IsParent bool default 0 not null,
Priority int default 0 not null,
Status tinyint default 0 not null,
CreatedOn datetime default current_timestamp  not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='产品分类';

use base;
drop table if exists ProdProp;
create table ProdProp(
Id int not null,
CatgId int not null,
ParentPid int not null,
ParentVid int not null,
Name nvarchar(100) not null,
Must bool default 0 not null,
Multi bool default 0 not null,
IsKeyProp bool default 0 not null,
IsSaleProp bool default 0 not null,
IsEnumProp bool default 0 not null,
IsItemProp bool default 0 not null,
IsColorProp bool default 0 not null,
IsInputProp bool default 0 not null,
ChildTemplate nvarchar(50) not null,
AllowAlias bool default 0 not null,
FeatureStr nvarchar(200),
Priority int default 0 not null,
Status tinyint default 0 not null,/*0为可用*/
CreatedOn datetime default current_timestamp  not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key (Id,CatgId)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='产品属性名';

use base;
drop table if exists ProdPropValue;
create table ProdPropValue(
Id int not null,
CatgId int not null,
PropId int not null,
Name nvarchar(100) not null,
Alias nvarchar(100),
PropName nvarchar(100) not null,
FeatureStr nvarchar(200),
Priority int default 0 not null,
Status tinyint default 0 not null,/*0为可用*/
CreatedOn datetime default current_timestamp  not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key (Id,CatgId,PropId)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='产品属性值';

drop table if exists Help;
create table Help(
Id int auto_increment not null,
Title nvarchar(200) default '' not null,
Content text,
GroupId tinyint default 0 not null,
Priority int default 0 not null,
Status bool default 1 not null,
CreatedOn datetime default current_timestamp  not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='帮助';

insert into help(title,Content) values('test','帮助');

drop table if exists Region ;
create table Region( 
Id varchar(6) not null,
Name nvarchar(50) not null,
GroupId tinyint default 0 not null,
ParentId varchar(6) default 0 not null,
CountryId int default 0 not null,
PostalCode varchar(6) not null,
CreatedOn datetime default current_timestamp  not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='行政区';

drop table if exists OAuth;
create table OAuth(
Id varchar(20) default '' not null,
Name nvarchar(30) default '' not null,
ClientId varchar(256) default '' not null,
ClientSecret varchar(300) default '' not null,
Scope varchar(300) default '' not null,
CallbackEndpoint varchar(300) default '' not null,
AuthBaseUrl varchar(300) default '' not null,
AuthorizationEndpoint varchar(300) default '' not null,
TokenEndpoint varchar(300) default '' not null,
UserEndpoint varchar(300) default '' not null,
Priority int default 0 not null,
Status bool default 1 not null,
CreatedOn datetime default current_timestamp  not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='OAuth';

insert into OAuth values('qq','','100275591','631a0befa8513bfeecde63eba55627dc','','http://www.ouere.com/external/auth/qq','https://graph.qq.com/oauth2.0/','authorize','token','me',1,1,now(),'',now());

insert into OAuth values('sina','','1276502867','7b38007ce811c520de74cfeaada7f524','','http://www.ouere.com/external/auth/sina','https://api.weibo.com/oauth2/','authorize','access_token','',2,1,now(),'',now());


drop table if exists LogisticsCompany ;
create table LogisticsCompany( 
Id int auto_increment not null,
Code varchar(6) not null,
Name nvarchar(50) not null,
Regex varchar(200)  null,
GroupId tinyint default 0 null,
Priority tinyint default 0 null,
Status bool default 1 not null,
CreatedOn datetime default current_timestamp  not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='物流公司';

drop table if exists SlideItem;
drop table if exists Slide;
create table Slide( 
Id int auto_increment not null,
GroupId tinyint default 0 not null,
Title nvarchar(50) default '' not null,
Width int default 200 not null,
Heigh int default 200 not null,
Thumb bool default 0 not null,
ThumbH int default 0 not null,
ThumbW int default 0 not null,
Description nvarchar(300),
Priority int default 200 not null,
Extra varchar(100),
Status bool default 1 not null,
CreatedOn datetime default current_timestamp not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='幻灯片';


create table SlideItem( 
Id int auto_increment not null,
SlideId int default 0 not null,
GroupId tinyint default 0 not null,
Title nvarchar(50) default '' not null,
LinkUrl nvarchar(300) default '' not null,
ImageSrc nvarchar(300) default '' not null,
ThumbSrc nvarchar(300) default '' not null,
Description nvarchar(300) default '',
Priority int default 0 not null,
StartedOn datetime default now() not null,
StoppedOn datetime default now() not null,
Extra varchar(100),
Status bool default 1 not null,
CreatedOn datetime default current_timestamp not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id),
foreign key(SlideId) references Slide(Id) on delete cascade
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='幻灯片条目';


drop table if exists PaymentEBank;
drop table if exists PaymentPlatform;
create table PaymentPlatform( 
Id int auto_increment not null,
Name nvarchar(200) default '' not null,
IconSrc varchar(200) default '' not null,
MerchantId varchar(100) default '' not null,
PrivateKey varchar(200) default '' not null,
PublicKey varchar(500) default '' not null,
GatewayUrl varchar(500) default '' not null,
CallbackUrl varchar(500) default '' not null,
NotifyUrl varchar(500) default '' not null,
QueryUrl varchar(500) default '' not null,
RefundUrl varchar(500) default '' not null,
Description nvarchar(8000),
Priority int default 0 not null,
Extra nvarchar(200),
Status bool default 0 not null,
CreatedOn datetime default current_timestamp not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='支付平台';

insert into PaymentPlatform values(0,'中国银联','','808080550007167','/pay.key/private.key','pay.key/public.key','http://payment-test.chinapay.com/pay/transget','','http://pay.silenthink.com/notify/chianpay','','','',1,'',1,now(),'',now());

drop table if exists PaymentBank;
create table PaymentBank( 
Id int auto_increment not null,
Name nvarchar(50) default '' not null,
IconSrc varchar(200) default '' not null,
Description nvarchar(800),
Status bool default 0 not null,
CreatedOn datetime default current_timestamp not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='支付平台银行';

create table PaymentEBank( 
Id int auto_increment not null,
BankId int default 0 not null,
PlatformId int default 0 not null,
GatewayCode varchar(100) default '' not null,
Priority int default 0 not null,
Description nvarchar(800),
Status bool default 0 not null,
CreatedOn datetime default current_timestamp not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id),
foreign key(BankId) references PaymentBank(Id) on delete cascade,
foreign key(PlatformId) references PaymentPlatform(Id) on delete cascade
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='支付平台在线支付银行';