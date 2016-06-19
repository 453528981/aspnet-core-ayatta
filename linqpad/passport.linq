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
drop database if exists Passport;

create database Passport;
*/

use Passport;

drop table if exists User;
create table User(
Id int auto_increment not null,
Guid char(32) not null,
Name nvarchar(50) not null,
Email varchar(60) default '',
Mobile varchar(12) default '',
Nickname nvarchar(50) default '',
Password varchar(50) not null,
PayPassword varchar(50),
Role tinyint default 0 not null,
`Level` tinyint default 0  not null,
`Limit` int default 0  not null,
Permit int default 0  not null,
Avatar nvarchar(50) default '',
Status tinyint default 0 not null,/*0为正常 255为删除*/
AuthedOn datetime,/*通过真实身份验证时间*/
CreatedBy nvarchar(50) default '',/*通过qq sina 等注册*/
CreatedOn datetime default current_timestamp not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id),
constraint UNIQUE (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户';

create index UserNameIndex on User(Name); 
create index UserEmailIndex on User(Email); 
create index UserMobileIndex on User(Mobile); 

drop table if exists UserProfile;
create table UserProfile(
Id int not null,
Code varchar(18),/*身份证号*/
Name nvarchar(50),/*真实姓名*/
Gender tinyint default 0 not null,/*0为保密 1为男 2为女*/
Marital tinyint default 0 not null,
Birthday date,
Phone varchar(16) default '',
Mobile varchar(12) default '',
RegionId varchar(10) default '',
Address nvarchar(200) default '',
SignUpIp varchar(16) default '',/*注册Ip*/
SignUpBy tinyint,/*0为通过用户名注册，1为通过邮箱注册，2为通过手机号码注册，3为通过手机短信注册*/
TraceCode varchar(36) default '',/*检测代码*/
LastSignInIp varchar(16) default '',/*最后一次登录Ip*/
LastSignInOn datetime default now() not null,/*最后一次登录时间*/
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户Profile';

insert into User(Guid,Name,Password,NickName,Role)values('a','420303865@qq.com','67337F56727C989BA9DD75A5FCEE8F0D','测试1',1);

insert into User(Guid,Name,Password,NickName,Role)values('b','453528981@qq.com','67337F56727C989BA9DD75A5FCEE8F0D','测试2',2);

insert into UserProfile(Id,Name)values(1,'test1');

insert into UserProfile(Id,Name)values(2,'test2');


/*通过第三方平台登录的用户信息*/
drop table if exists UserOAuth;
create table UserOAuth(
Id int not null,
OpenId varchar(36) not null,
OpenName nvarchar(50) default '' not null,
Provider nvarchar(20) default '' not null,
Scope varchar(500) default '' not null,
AccessToken varchar(50) default '' not null,
RefreshToken varchar(50) default '' not null,
ExpirationTime datetime not null,
Extra nvarchar(200),
CreatedOn datetime default current_timestamp not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户OAuth';


drop table if exists UserFavorite;
create table UserFavorite(
Id int auto_increment not null,
UserId int default 0 not null,
GroupId tinyint default 0 not null,/*类型 0商品 2品牌 3店铺*/ 
FKey nvarchar(50) default '',
FValue nvarchar(200) default '',
FExtra nvarchar(200) default '',
CreatedOn datetime default current_timestamp not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id),
constraint UNIQUE (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户关注';


drop table if exists UserAddress;
create table UserAddress(
Id int auto_increment not null,
UserId int default 0 not null,
GroupId tinyint default 0 not null,/*地址类型 0为收货地址 2收货地址 3退货地址*/ 
Consignee nvarchar(50) default '' not null,
CompanyName nvarchar(100) default '',
CountryId int default 0 not null,
RegionId varchar(6) not null,
Province nvarchar(20) not null,
City nvarchar(20) not null,
District nvarchar(50) not null,
Address nvarchar(200) not null,
PostalCode varchar(20) not null,
Phone varchar(50) default '',
Mobile varchar(50) default '',
IsDefault bit default 0 not null,
CreatedOn datetime default current_timestamp not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id),
constraint UNIQUE (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户地址';


drop table if exists UserInvoice;
create table UserInvoice(
Id int auto_increment not null,
UserId int default 0 not null,
GroupId tinyint not null,/*0为普通 1增值*/ 
Title nvarchar(50) not null,
Content varchar(50),
IsDefault bit default 0 not null,
CreatedOn datetime default current_timestamp not null,
ModifiedBy nvarchar(50) default '',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null,
primary key(Id),
constraint UNIQUE (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户发票';