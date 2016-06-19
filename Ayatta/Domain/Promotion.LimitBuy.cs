using System;
using ProtoBuf;
using System.Collections.Generic;
namespace Ayatta.Domain
{
    /// <summary>
    /// 促销 http://bbs.taobao.com/catalog/thread/16543510-264265243.htm
    /// </summary>
    public partial class Promotion
    {

        ///<summary>
        /// 限购
        /// http://bbs.taobao.com/catalog/thread/16543510-264271770.htm
        ///created on 2016-05-22 00:48:12
        ///<summary>
        [ProtoContract]
        public class LimitBuy : IEntity<int>
        {

            ///<summary>
            /// Id
            ///<summary>
            [ProtoMember(1)]
            public int Id { get; set; }

            ///<summary>
            /// 商品名称
            ///<summary>
            [ProtoMember(2)]
            public string Name { get; set; }

            ///<summary>
            /// 商品标题
            ///<summary>
            [ProtoMember(3)]
            public string Title { get; set; }

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
            /// 商品Id
            ///<summary>
            [ProtoMember(7)]
            public int ItemId { get; set; }

            ///<summary>
            /// 限购数量
            ///<summary>
            [ProtoMember(8)]
            public int Value { get; set; }

            ///<summary>
            /// 卖家Id
            ///<summary>
            [ProtoMember(9)]
            public int SellerId { get; set; }

            ///<summary>
            /// 状态 true为可用
            ///<summary>
            [ProtoMember(10)]
            public bool Status { get; set; }

            ///<summary>
            /// 创建时间
            ///<summary>
            [ProtoMember(11)]
            public DateTime CreatedOn { get; set; }

            ///<summary>
            /// 最后一次编辑者
            ///<summary>
            [ProtoMember(12)]
            public string ModifiedBy { get; set; }

            ///<summary>
            /// 最后一次编辑时间
            ///<summary>
            [ProtoMember(13)]
            public DateTime ModifiedOn { get; set; }

            /// <summary>
            /// 判断限购是否有效
            /// </summary>
            /// <returns></returns>
            public bool IsValid()
            {
                var now = DateTime.Now;
                return Status && StartedOn < now && now < StoppedOn && Value > 0;
            }
        }
    }

    /*
     
            1.  限购件数最多可以设置为多少？
            限购件数最多可配置为200件。
		
            2.  限购时段最长可以设置为多久？
            限购时段最长可设为30天。
		
            3.  限购开始时间不能晚于当前时间多少天？
            限购开始时间不能晚于当前时间90天。
		
            4. 每个商品最多可以设置多少条限购规则？
            每个商品最多同时设置5条有效的限购规则；
            有效的限购规则是指：正在进行中和即将开始的限购
		
            ●  商家可对自己的商品配置在时间段内每个ID限购该几件。
            ●  该限购与该商品的价格无关，只与消费者已经购买了该商品几件相关
            ●  限购逻辑更严谨：消费者对商品下单，件数算入限购；如果订单未付款且关闭了，那么会回补限购数，避免消费者由于拍错规格码色等导致的误限购
            ●  达到限购数不能下单
           
    */
}

