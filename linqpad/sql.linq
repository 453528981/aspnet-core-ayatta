<Query Kind="SQL">
  <Connection>
    <ID>a093c89a-b903-4883-b955-f2eeed2e9931</ID>
    <Driver Assembly="IQDriver" PublicKeyToken="5b59726538a49684">IQDriver.IQDriver</Driver>
    <Provider>Devart.Data.MySql</Provider>
    <CustomCxString>Server=127.0.0.1;Port=3306;Database=store;Uid=root</CustomCxString>
    <NoPluralization>true</NoPluralization>
    <Server>127.0.0.1</Server>
    <Database>store</Database>
    <UserName>root</UserName>
    <DriverData>
      <StripUnderscores>false</StripUnderscores>
      <QuietenAllCaps>false</QuietenAllCaps>
      <Port>3306</Port>
    </DriverData>
  </Connection>
</Query>

DROP DATABASE IF EXISTS Store;

CREATE DATABASE Store;

/*GRANT ALL PRIVILEGES ON Store.* to sa@localhost IDENTIFIED BY 'sa';*/


USE Store;

DROP TABLE IF EXISTS Promotion;

CREATE TABLE Promotion (
  Id int unsigned auto_increment not null,
  Name nvarchar(200) not null,
  Title nvarchar(200) not null,
  Description nvarchar(500),
  BeginTime datetime not null,
  EndTime datetime not null
  
  PRIMARY KEY(Id)
);

drop table if exists user;
create table user(
id int unsigned auto_increment not null,
guid char(32) not null default '',
role tinyint unsigned not null default 0,
level tinyint unsigned not null default 0,
name varchar(50) not null default '',
nickname nvarchar(50) not null default '',
password char(32) not null default '',
pay_password char(32) not null default '',
email nvarchar(200) not null default '',
mobile varchar(20) not null default '',
avatar varchar(200) not null default '',
authenticated tinyint(1) default 0,
authenticated_on timestamp not null default 0,
status tinyint unsigned not null default 0,
modified_by nvarchar(50) not null default '',
modified_on timestamp not null default 0,
created_by nvarchar(50) not null default '',
created_on timestamp not null default current_timestamp,
primary key (id),
unique key (name)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

insert into user(name)values('test');

update user set guid='guid',authenticated=10 where id=1

show full columns from store.test

ALTER TABLE test
ADD COLUMN xx  int unsigned not NULL
DEFAULT 0
COMMENT '对应的书籍id' AFTER bool;

show columns from   store.test

show full columns from store.test

insert into ProdCatg select id,name,parent_id,is_parent,priority,0,'2016-05-05','sys','2016-05-05' from store.spu_catg

insert into ProdProp select id,catg_id,parent_pid,parent_vid,name,must,multi,is_key_prop,is_sale_prop,is_enum_prop,is_item_prop,is_color_prop,
is_input_prop,child_template,0,'',priority,0,'2016-05-05','sys','2016-05-05' from store.spu_prop

insert into prodpropvalue select id,catg_id,prop_id,name,name_alias,prop_name,'',priority,0,'2016-05-05','sys','2016-05-05' from store.spu_prop_value