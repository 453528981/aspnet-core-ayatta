using System;
using ProtoBuf;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    public static partial class Prod
    {

        ///<summary>
        /// Sku
        ///created on 2016-05-13 13:36:59
        ///<summary>
        [ProtoContract]
        public class Sku : IEntity<int>
        {

            ///<summary>
            /// 
            ///<summary>
            [ProtoMember(1)]
            public int Id { get; set; }

            ///<summary>
            /// SpuId
            ///<summary>
            [ProtoMember(2)]
            public int SpuId { get; set; }

            ///<summary>
            /// ItemId
            ///<summary>
            [ProtoMember(3)]
            public int ItemId { get; set; }

            ///<summary>
            /// 商家Id
            ///<summary>
            [ProtoMember(4)]
            public int UserId { get; set; }

            ///<summary>
            /// 最小类目id
            ///<summary>
            [ProtoMember(5)]
            public int CatgId { get; set; }

            ///<summary>
            /// 根类目id
            ///<summary>
            [ProtoMember(6)]
            public int CatgRId { get; set; }

            ///<summary>
            /// 中间类目id
            ///<summary>
            [ProtoMember(7)]
            public string CatgMId { get; set; }

            ///<summary>
            /// 商家设置的外部id
            ///<summary>
            [ProtoMember(8)]
            public string Code { get; set; }

            ///<summary>
            /// 条形码
            ///<summary>
            [ProtoMember(9)]
            public string Barcode { get; set; }

            ///<summary>
            /// 品牌Id
            ///<summary>
            [ProtoMember(10)]
            public int BrandId { get; set; }

            ///<summary>
            /// Sku库存数量
            ///<summary>
            [ProtoMember(11)]
            public int Stock { get; set; }

            ///<summary>
            /// Sku价格
            ///<summary>
            [ProtoMember(12)]
            public decimal Price { get; set; }

            ///<summary>
            /// Sku app 价格
            ///<summary>
            [ProtoMember(13)]
            public decimal AppPrice { get; set; }

            ///<summary>
            /// Sku属性Id 格式：pid:vid;pid:vid
            ///<summary>
            [ProtoMember(14)]
            public string PropId { get; set; }

            ///<summary>
            /// Sku属性值 格式 pid:vid:pname:vname;pid:vid:pname:vname
            ///<summary>
            [JsonIgnore]
            [ProtoMember(15)]
            public string PropStr { get; set; }

            ///<summary>
            /// 销售数量
            ///<summary>
            [ProtoMember(16)]
            public int SaleCount { get; set; }

            ///<summary>
            /// 状态 0为可用
            ///<summary>
            [ProtoMember(17)]
            public Status Status { get; set; }

            ///<summary>
            /// 创建时间
            ///<summary>
            [ProtoMember(18)]
            public DateTime CreatedOn { get; set; }

            ///<summary>
            /// 最后一次编辑者
            ///<summary>
            [ProtoMember(19)]
            public string ModifiedBy { get; set; }

            ///<summary>
            /// 最后一次编辑时间
            ///<summary>
            [ProtoMember(20)]
            public DateTime ModifiedOn { get; set; }

            public virtual IList<Prop> Props
            {
                get
                {
                    var list = new List<Prop>();
                    if (!string.IsNullOrEmpty(PropStr))
                    {
                        var array = PropStr.Split(';');
                        foreach (var s in array)
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                var temp = s.Split(':');
                                var prop = new Prop();
                                prop.PId = Convert.ToInt32(temp[0]);
                                prop.VId = Convert.ToInt32(temp[1]);
                                prop.PName = temp[2];
                                prop.VName = temp[3];
                                list.Add(prop);
                            }
                        }
                    }
                    return list;
                }
            }

            public virtual string[] PropTexts
            {
                get
                {
                    return !string.IsNullOrEmpty(PropStr) ? PropStr.Split(';').Select(o => o.Split(':')[2].Trim() + "：" + o.Split(':')[3].Trim()).ToArray() : null;
                }
            }
        }
    }
}