/*
drop database if exists promotion;

create database promotion;
*/

use promotion;

drop table if exists Normal;

create table Normal(
Id int auto_increment not null comment 'Id',
Name nvarchar(200) not null comment '活动名称',
Title nvarchar(300) not null comment '活动标题',
StartedOn datetime not null comment '开始时间',
StoppedOn datetime not null comment '结束时间',
Plateform tinyint default 0 not null comment '活动适用平台',
Global bool default 0 not null comment '适用于(全场店铺)所有商品',
Discount bool default 0 not null comment 'true为满件折 false为满元减', 
LimitBy tinyint not null comment '用户参与活动限制', 
LimitValue int not null comment '用户参与活动限制值 Limit为true时有效', 
WarmUp int default 1 not null comment '提前预热天数', 
Picture varchar(300) comment '标准版 活动图片', 
ExternalUrl varchar(300) comment '豪华版 专辑地址', 
Infinite bool default 0 not null comment '上不封顶(当规则为满元减且只有一级时 该值可为true)', 
ItemId varchar(4000) comment 'Global==false时为包含的商品(以,分隔) Global==true时为排除的商品(以,分隔)',
FreightFree bool default 0 not null comment '免运费', 
FreightFreeExclude varchar(200) comment ' 免运费排除在外的地区(以,分隔)', 
SellerId int not null comment '卖家Id', 
Status bool default 0 not null comment '状态 1为可用',
CreatedOn datetime default current_timestamp  not null comment '创建时间',
ModifiedBy nvarchar(50) default '' comment '最后一次编辑者',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='店铺活动';
use promotion;
drop table if exists NormalRule;
create table NormalRule(
Id int auto_increment not null comment 'Id',
NormalId int not null comment '卖家Id', 
Upon decimal(8,2) not null comment '临界值', 
Value decimal(8,2) not null comment '满减/满折值', 
SendGift bit not null comment '送赠品', 
GiftJson nvarchar(800) comment '赠品信息 Json格式', 
SendCoupon bit not null comment '送店铺优惠券', 
CouponJson nvarchar(800) comment '优惠券信息 Json格式', 
SellerId int not null comment '卖家Id', 
Status bool default 0 not null comment '状态 1为可用',
CreatedOn datetime default current_timestamp  not null comment '创建时间',
ModifiedBy nvarchar(50) default '' comment '最后一次编辑者',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='店铺活动规则';

use promotion;
drop table if exists Package;
create table Package(
Id int auto_increment not null comment 'Id',
Name nvarchar(200) not null comment '套餐标题',
Title nvarchar(300) not null comment '套餐品名',
StartedOn datetime not null comment '开始时间',
StoppedOn datetime not null comment '结束时间',
Plateform tinyint default 0 not null comment '活动适用平台',
Fixed bool default 0 not null comment '固定组合套餐 商品打包成套餐销售 消费者打包购买 自选商品套餐 套餐中的附属商品 消费者可以通过复选框的方式有选择的购买',
ItemId int not null comment '主商品Id',
ItemName nvarchar(200) comment '主商品名称' ,
ItemPrice decimal(8,2) not null comment ' 主商品搭配价格 0为默认如果不设置搭配价 则执行在售价(适用于有多个不同Sku 如果没有sku则可设置一个搭配价格)',
ItemPictrue nvarchar(300) comment '主商品搭配图' ,
LimitBy tinyint not null comment '用户参与活动限制' ,
LimitValue int not null comment ' 用户参与活动限制值 Limit为true时有效' ,
SellerId int not null comment '卖家Id', 
Status bool default 0 not null comment '状态 1为可用',
CreatedOn datetime default current_timestamp  not null comment '创建时间',
ModifiedBy nvarchar(50) default '' comment '最后一次编辑者',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='搭配组合套餐';

use promotion;
drop table if exists PackageItem;
create table PackageItem(
Id int auto_increment not null comment 'Id',
PackageId int not null comment '搭配组合套餐Id',
MainId int not null comment '主商品Id',
SkuId int not null comment 'SkuId',
ItemId int not null comment 'ItemId',
Name nvarchar(200) comment '附属商品名称',
Price decimal(8,2) default 0 not null comment '附属商品价格 0为默认如果不设置搭配价 则执行在售价',
Picture nvarchar(300) comment '附属商品图片',
Selected bool not null comment '默认勾选',
Priority int default 0 not null comment '排序 从大到小',
SellerId int not null comment '卖家Id', 
Status bool default 0 not null comment '状态 1为可用',
CreatedOn datetime default current_timestamp  not null comment '创建时间',
ModifiedBy nvarchar(50) default '' comment '最后一次编辑者',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='套餐附属商品';

use promotion;
drop table if exists LimitBuy;
create table LimitBuy(
Id int auto_increment not null comment 'Id',
Name nvarchar(200) not null comment '商品名称',
Title nvarchar(300) not null comment '商品标题',
StartedOn datetime not null comment '开始时间',
StoppedOn datetime not null comment '结束时间',
Plateform tinyint default 0 not null comment '活动适用平台',
ItemId int default 0 not null comment '商品Id',
Value int default 0 not null comment '限购数量',
SellerId int not null comment '卖家Id', 
Status bool default 0 not null comment '状态 1为可用',
CreatedOn datetime default current_timestamp  not null comment '创建时间',
ModifiedBy nvarchar(50) default '' comment '最后一次编辑者',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='限购';

drop table if exists SpecialPrice;
create table SpecialPrice(
Id int auto_increment not null comment 'Id',
Name nvarchar(200) not null comment '商品名称',
Title nvarchar(300) not null comment '商品标题',
StartedOn datetime not null comment '开始时间',
StoppedOn datetime not null comment '结束时间',
Plateform tinyint default 0 not null comment '活动适用平台',
CategId tinyint not null comment 'A打折  B减价  C促销价 活动创建后,优惠方式将不能修改',
FreightFree bool not null comment '免运费',
FreightFreeExclude varchar(200) comment '免运费排除在外的地区(以,分隔)',
SellerId int not null comment '卖家Id', 
Status bool default 0 not null comment '状态 1为可用',
CreatedOn datetime default current_timestamp  not null comment '创建时间',
ModifiedBy nvarchar(50) default '' comment '最后一次编辑者',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='特价';

drop table if exists SpecialPriceItem;
create table SpecialPriceItem(
Id int auto_increment not null comment 'Id',
SpecialPriceId int not null comment '特价Id',
ItemId int not null comment '商品Id',
Global bit not null comment '统一设置优惠(商品维度)',
Value decimal(8,2) comment '统一设置优惠值(商品维度)',
LimitBy tinyint not null comment '用户参与活动限制',
LimitValue int not null comment '用户参与活动限制值 Limit为true时有效',
SkuJson nvarchar(2000) comment '对Sku设置的优惠信息 Json格式',
SellerId int not null comment '卖家Id', 
Status bool default 0 not null comment '状态 1为可用',
CreatedOn datetime default current_timestamp  not null comment '创建时间',
ModifiedBy nvarchar(50) default '' comment '最后一次编辑者',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='特价条目';

drop table if exists Cart;
create table Cart(
Id int auto_increment not null comment 'Id',
Name nvarchar(200) not null comment '商品名称',
Title nvarchar(300) not null comment '商品标题',
StartedOn datetime not null comment '开始时间',
StoppedOn datetime not null comment '结束时间',
Plateform tinyint default 0 not null comment '活动适用平台',
PayMethod tinyint not null comment '适用付款方式',
ApplyTo tinyint not null comment '促销效果作用于',
Discount bool not null comment 'true为打折 false为减元',
Value decimal(8,2) not null comment '促销值 减x元 打x折',
LimitBy tinyint not null comment '用户参与活动限制',
LimitValue int not null comment '用户参与活动限制值 Limit为true时有效',
ProdCatgId varchar(200) comment '商品类目Id *为匹配所有类目 如需匹配部分类目 使用","分隔',
ProdBrandId varchar(200) comment '商品品牌Id *为匹配所有品牌 如需匹配部分品牌 使用","分隔',
ProdItemId varchar(2000) comment '商品ItemId *为匹配所有商品 如需匹配部分商品 使用","分隔',
RegionId varchar(200) comment '区域Id *为匹配所有区 如需匹配部分区 使用","分隔',
UserId varchar(2000) comment 'UserId *为匹配所有用户 如需匹配部分用户 使用","分隔',
SellerId int not null comment '卖家Id', 
Status bool default 0 not null comment '状态 1为可用',
CreatedOn datetime default current_timestamp  not null comment '创建时间',
ModifiedBy nvarchar(50) default '' comment '最后一次编辑者',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='购物车促销';

drop table if exists CartRule;
create table CartRule(
Id int auto_increment not null comment 'Id',
CartId int not null comment '购物车促销Id',
StartedOn datetime comment '开始时间',
StoppedOn datetime comment '结束时间',
Plateform tinyint default 0 not null comment '活动适用平台',
Calc tinyint not null comment '计算方式',
Value varchar(2000) not null comment '参数值',
Priority int default 0 not null comment '优先顺序 从大到小',
SellerId int not null comment '卖家Id', 
Status bool default 0 not null comment '状态 1为可用',
CreatedOn datetime default current_timestamp  not null comment '创建时间',
ModifiedBy nvarchar(50) default '' comment '最后一次编辑者',
ModifiedOn timestamp on update current_timestamp default current_timestamp not null comment '最后一次编辑时间',
primary key (Id)
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='购物车促销';