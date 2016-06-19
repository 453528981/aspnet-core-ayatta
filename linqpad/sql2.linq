<Query Kind="SQL">
  <Connection>
    <ID>49726ddb-2eff-4022-a02e-97b9acb81dd2</ID>
    <Persist>true</Persist>
    <Driver Assembly="IQDriver" PublicKeyToken="5b59726538a49684">IQDriver.IQDriver</Driver>
    <Provider>Devart.Data.MySql</Provider>
    <CustomCxString>Server=127.0.0.1;Database=store;Uid=root</CustomCxString>
    <UserName>root</UserName>
    <Server>127.0.0.1</Server>
    <Database>store</Database>
    <NoPluralization>true</NoPluralization>
    <NoCapitalization>true</NoCapitalization>
    <DriverData>
      <StripUnderscores>false</StripUnderscores>
      <QuietenAllCaps>false</QuietenAllCaps>
    </DriverData>
  </Connection>
</Query>

drop table if exists Test;
create table Test(
Id serial primary key,
Name varchar(50) not null,
Birthday date not null default '1970-01-01',
CreatedBy nvarchar(50) not null default '',
CreatedAt timestamp not null default now(),
UpdatedAt timestamp not null ,
  UNIQUE KEY Name (Name) 
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

insert into test(name,CreatedAt)values('aa',now());

update test set name='eee00',CreatedAt=now() where id=1