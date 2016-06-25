﻿using System;
using ProtoBuf;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    public static partial class Prod
    {
        public class Mini : IEntity<int>
        {
            ///<summary>
            /// Id
            ///<summary>
            [ProtoMember(1)]
            public int Id { get; set; }

            ///<summary>
            /// SpuId
            ///<summary>
            [ProtoMember(2)]
            public int SpuId { get; set; }

            ///<summary>
            /// 商家Id
            ///<summary>
            [ProtoMember(3)]
            public int UserId { get; set; }

            ///<summary>
            /// 最小类目id
            ///<summary>
            [ProtoMember(4)]
            public int CatgId { get; set; }

            ///<summary>
            /// 根类目id
            ///<summary>
            [ProtoMember(5)]
            public int CatgRId { get; set; }

            ///<summary>
            /// 商家设置的外部id
            ///<summary>
            [ProtoMember(7)]
            public string Code { get; set; }

            ///<summary>
            /// 商品名称,不能超过60字节
            ///<summary>
            [ProtoMember(8)]
            public string Name { get; set; }

            ///<summary>
            /// 标题
            ///<summary>
            [ProtoMember(9)]
            public string Title { get; set; }

            ///<summary>
            /// 商品库存数量
            ///<summary>
            [ProtoMember(10)]
            public int Stock { get; set; }

            ///<summary>
            /// 商品价格
            ///<summary>
            [ProtoMember(11)]
            public decimal Price { get; set; }

            ///<summary>
            /// app商品价格
            ///<summary>
            [ProtoMember(12)]
            public decimal AppPrice { get; set; }

            ///<summary>
            /// 商品建议零售价格
            ///<summary>
            [ProtoMember(13)]
            public decimal RetailPrice { get; set; }

            ///<summary>
            /// 条形码
            ///<summary>
            [ProtoMember(14)]
            public string Barcode { get; set; }

            ///<summary>
            /// 品牌Id
            ///<summary>
            [ProtoMember(15)]
            public int BrandId { get; set; }

            ///<summary>
            /// 品牌名
            ///<summary>
            [ProtoMember(16)]
            public string BrandName { get; set; }

            ///<summary>
            /// 关键字
            ///<summary>
            [ProtoMember(17)]
            public string Keyword { get; set; }

            ///<summary>
            /// 商品属性Id 格式：pid:vid;pid:vid
            ///<summary>
            [JsonIgnore]
            [ProtoMember(18)]
            public string PropId { get; set; }

            ///<summary>
            /// 商品属性值 格式 pid:vid:pname:vname;pid:vid:pname:vname
            ///<summary>
            [JsonIgnore]
            [ProtoMember(19)]
            public string PropStr { get; set; }

            ///<summary>
            /// 商品输入属性Id
            ///<summary>
            [JsonIgnore]
            [ProtoMember(21)]
            public string InputId { get; set; }

            ///<summary>
            /// 商品输入属性值
            ///<summary>
            [JsonIgnore]
            [ProtoMember(22)]
            public string InputStr { get; set; }

            ///<summary>
            /// 商品概要
            ///<summary>
            [ProtoMember(23)]
            public string Summary { get; set; }

            ///<summary>
            /// 商品主图片地址
            ///<summary>
            [ProtoMember(24)]
            public string Picture { get; set; }

            ///<summary>
            /// 商品图片列表(包括主图)
            ///<summary>
            [JsonIgnore]
            [ProtoMember(25)]
            public string ItemImgStr { get; set; }

            ///<summary>
            /// 商品属性图片列表
            ///<summary>
            [JsonIgnore]
            [ProtoMember(26)]
            public string PropImgStr { get; set; }

            ///<summary>
            /// 状态 0为可用
            ///<summary>
            [ProtoMember(52)]
            public Status Status { get; set; }

            /// <summary>
            /// Sku列表
            /// </summary>
            [ProtoMember(100)]
            public virtual IList<Sku> Skus { get; set; }

            /// <summary>
            /// 属性
            /// </summary>
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
                    if (Skus != null && Skus.Any())
                    {
                        list.AddRange(Skus.SelectMany(x => x.Props));
                    }
                    return list;
                }
            }

            /// <summary>
            /// 图片
            /// </summary>
            public virtual IList<string> Imgs
            {
                get
                {
                    var list = new List<string>(5);
                    if (!string.IsNullOrEmpty(ItemImgStr))
                    {
                        list.AddRange(ItemImgStr.Split(';').Where(x => !string.IsNullOrEmpty(x)));
                    }
                    return list;
                }
            }

            /// <summary>
            /// 颜色图片
            /// </summary>
            public virtual IDictionary<string, string> PropImgs
            {
                get
                {
                    var dic = new Dictionary<string, string>();

                    if (!string.IsNullOrEmpty(PropImgStr))
                    {
                        dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(PropImgStr);
                    }
                    return dic;
                }
            }

            [ProtoContract]
            public class Sku : IEntity<int>
            {

                ///<summary>
                /// Id
                ///<summary>
                [ProtoMember(1)]
                public int Id { get; set; }

                ///<summary>
                /// 商家设置的外部id
                ///<summary>
                [ProtoMember(2)]
                public string Code { get; set; }

                ///<summary>
                /// 商品条形码
                ///<summary>
                [ProtoMember(2)]
                public string Barcode { get; set; }

                ///<summary>
                /// 商品库存数量
                ///<summary>
                [ProtoMember(4)]
                public int Stock { get; set; }

                ///<summary>
                /// 商品价格
                ///<summary>
                [ProtoMember(5)]
                public decimal Price { get; set; }

                ///<summary>
                /// Sku app 价格
                ///<summary>
                [ProtoMember(6)]
                public decimal AppPrice { get; set; }

                ///<summary>
                /// 商品属性Id 格式：pid:vid;pid:vid
                ///<summary>
                [ProtoMember(7)]
                public string PropId { get; set; }

                ///<summary>
                /// 商品属性值 格式 pid:vid:pname:vname;pid:vid:pname:vname
                ///<summary>
                [JsonIgnore]
                [ProtoMember(8)]
                public string PropStr { get; set; }

                ///<summary>
                /// 状态 0为可用
                ///<summary>
                [ProtoMember(9)]
                public Status Status { get; set; }

                [JsonIgnore]
                public virtual IList<Prop> Props
                {
                    get
                    {
                        var list = new List<Prop>(7);
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
                                    prop.Extra = "sku";
                                    list.Add(prop);
                                }
                            }
                        }
                        return list;
                    }
                }
            }

        }
    }
}