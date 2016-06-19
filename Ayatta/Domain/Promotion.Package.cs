using System;
using ProtoBuf;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    /// <summary>
    /// 促销 http://bbs.taobao.com/catalog/thread/16543510-264265243.htm
    /// </summary>
    public partial class Promotion
    {

        ///<summary>
        /// 搭配组合套餐 
        /// http://bbs.taobao.com/catalog/thread/16543510-264271296.htm
        ///created on 2016-05-22 01:06:49
        ///<summary>
        [ProtoContract]
        public class Package : IEntity<int>
        {

            ///<summary>
            /// Id
            ///<summary>
            [ProtoMember(1)]
            public int Id { get; set; }

            ///<summary>
            /// 套餐标题
            ///<summary>
            [ProtoMember(2)]
            public string Name { get; set; }

            ///<summary>
            /// 套餐描述
            ///<summary>
            [ProtoMember(3)]
            public string Summary { get; set; }

            ///<summary>
            /// 开始时间
            ///<summary>
            [ProtoMember(4)]
            public DateTime StartedOn { get; set; }

            ///<summary>
            /// 结束时间
            ///<summary>
            [ProtoMember(5)]
            public DateTime StoppedOn { get; set; }

            ///<summary>
            /// 活动适用平台
            ///<summary>
            [ProtoMember(6)]
            public Plateform Plateform { get; set; }

            ///<summary>
            /// 固定组合套餐 商品打包成套餐销售 消费者打包购买 自选商品套餐 套餐中的附属商品 消费者可以通过复选框的方式有选择的购买
            ///<summary>
            [ProtoMember(7)]
            public bool Fixed { get; set; }

            ///<summary>
            /// 主商品Id
            ///<summary>
            [ProtoMember(8)]
            public int ItemId { get; set; }

            ///<summary>
            /// 主商品名称
            ///<summary>
            [ProtoMember(9)]
            public string ItemName { get; set; }

            ///<summary>
            ///  主商品搭配价格 0为默认如果不设置搭配价 则执行在售价(适用于有多个不同Sku 如果没有sku则可设置一个搭配价格)
            ///<summary>
            [ProtoMember(10)]
            public decimal ItemPrice { get; set; }

            ///<summary>
            /// 主商品搭配图
            ///<summary>
            [ProtoMember(11)]
            public string ItemPictrue { get; set; }

            ///<summary>
            /// 用户参与活动限制
            ///<summary>
            [ProtoMember(12)]
            public LimitBy LimitBy { get; set; }

            ///<summary>
            ///  用户参与活动限制值 Limit为true时有效
            ///<summary>
            [ProtoMember(13)]
            public int LimitValue { get; set; }

            ///<summary>
            /// 卖家Id
            ///<summary>
            [ProtoMember(14)]
            public int SellerId { get; set; }

            ///<summary>
            /// 状态 1为可用
            ///<summary>
            [ProtoMember(15)]
            public bool Status { get; set; }

            ///<summary>
            /// 创建时间
            ///<summary>
            [ProtoMember(16)]
            public DateTime CreatedOn { get; set; }

            ///<summary>
            /// 最后一次编辑者
            ///<summary>
            [ProtoMember(17)]
            public string ModifiedBy { get; set; }

            ///<summary>
            /// 最后一次编辑时间
            ///<summary>
            [ProtoMember(18)]
            public DateTime ModifiedOn { get; set; }

            /// <summary>
            /// 搭配组合套餐附属商品 最多可以添加8个
            /// </summary>
            [ProtoMember(19)]
            public virtual IList<Item> Items { get; set; }

            /// <summary>
            /// 判断活动是否有效
            /// </summary>
            /// <returns></returns>
            public bool IsValid()
            {
                var now = DateTime.Now;
                return Status && StartedOn < now && now < StoppedOn && Items.Any(x => x.Status);
            }

            ///<summary>
            /// PackageItem
            ///created on 2016-05-22 01:09:44
            ///<summary>
            [ProtoContract]
            public class Item : IEntity<int>
            {

                ///<summary>
                /// Id
                ///<summary>
                [ProtoMember(1)]
                public int Id { get; set; }

                ///<summary>
                /// 搭配组合套餐Id
                ///<summary>
                [ProtoMember(2)]
                public int PackageId { get; set; }

                ///<summary>
                /// 主商品Id
                ///<summary>
                [ProtoMember(3)]
                public int MainId { get; set; }

                ///<summary>
                /// SkuId
                ///<summary>
                [ProtoMember(4)]
                public int SkuId { get; set; }

                ///<summary>
                /// ItemId
                ///<summary>
                [ProtoMember(5)]
                public int ItemId { get; set; }

                ///<summary>
                /// 附属商品名称
                ///<summary>
                [ProtoMember(6)]
                public string Name { get; set; }

                ///<summary>
                /// 附属商品价格 0为默认如果不设置搭配价 则执行在售价
                ///<summary>
                [ProtoMember(7)]
                public decimal Price { get; set; }

                ///<summary>
                /// 附属商品图片
                ///<summary>
                [ProtoMember(8)]
                public string Picture { get; set; }

                ///<summary>
                /// 默认勾选
                ///<summary>
                [ProtoMember(9)]
                public bool Selected { get; set; }

                ///<summary>
                /// 排序 从大到小
                ///<summary>
                [ProtoMember(10)]
                public int Priority { get; set; }

                ///<summary>
                /// 卖家Id
                ///<summary>
                [ProtoMember(11)]
                public int SellerId { get; set; }

                ///<summary>
                /// 状态 1为可用
                ///<summary>
                [ProtoMember(12)]
                public bool Status { get; set; }

                ///<summary>
                /// 创建时间
                ///<summary>
                [ProtoMember(13)]
                public DateTime CreatedOn { get; set; }

                ///<summary>
                /// 最后一次编辑者
                ///<summary>
                [ProtoMember(14)]
                public string ModifiedBy { get; set; }

                ///<summary>
                /// 最后一次编辑时间
                ///<summary>
                [ProtoMember(15)]
                public DateTime ModifiedOn { get; set; }
            }

        }
    }
}

