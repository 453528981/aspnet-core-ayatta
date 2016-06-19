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
        /// 购物车促销
        ///created on 2016-05-22 00:36:45
        ///<summary>
        [ProtoContract]
        public class Cart : IEntity<int>
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
            /// 适用付款方式
            ///<summary>
            [ProtoMember(7)]
            public PayMethod PayMethod { get; set; }

            ///<summary>
            /// 促销效果作用于
            ///<summary>
            [ProtoMember(8)]
            public ApplyTo ApplyTo { get; set; }

            ///<summary>
            /// true为打折 false为减元
            ///<summary>
            [ProtoMember(9)]
            public bool Discount { get; set; }

            ///<summary>
            /// 促销值 减x元 打x折
            ///<summary>
            [ProtoMember(10)]
            public decimal Value { get; set; }

            ///<summary>
            /// 用户参与活动限制
            ///<summary>
            [ProtoMember(11)]
            public LimitBy LimitBy { get; set; }

            ///<summary>
            /// 用户参与活动限制值 Limit为true时有效
            ///<summary>
            [ProtoMember(12)]
            public int LimitValue { get; set; }

            ///<summary>
            /// 商品类目Id *为匹配所有类目 如需匹配部分类目 使用","分隔
            ///<summary>
            [ProtoMember(13)]
            public string ProdCatgId { get; set; }

            ///<summary>
            /// 商品品牌Id *为匹配所有品牌 如需匹配部分品牌 使用","分隔
            ///<summary>
            [ProtoMember(14)]
            public string ProdBrandId { get; set; }

            ///<summary>
            /// 商品ItemId *为匹配所有商品 如需匹配部分商品 使用","分隔
            ///<summary>
            [ProtoMember(15)]
            public string ProdItemId { get; set; }

            ///<summary>
            /// 区域Id *为匹配所有区 如需匹配部分区 使用","分隔
            ///<summary>
            [ProtoMember(16)]
            public string RegionId { get; set; }

            ///<summary>
            /// UserId *为匹配所有用户 如需匹配部分用户 使用","分隔
            ///<summary>
            [ProtoMember(17)]
            public string UserId { get; set; }

            ///<summary>
            /// 卖家Id
            ///<summary>
            [ProtoMember(18)]
            public int SellerId { get; set; }

            ///<summary>
            /// 状态 1为可用
            ///<summary>
            [ProtoMember(19)]
            public bool Status { get; set; }

            ///<summary>
            /// 创建时间
            ///<summary>
            [ProtoMember(20)]
            public DateTime CreatedOn { get; set; }

            ///<summary>
            /// 最后一次编辑者
            ///<summary>
            [ProtoMember(21)]
            public string ModifiedBy { get; set; }

            ///<summary>
            /// 最后一次编辑时间
            ///<summary>
            [ProtoMember(22)]
            public DateTime ModifiedOn { get; set; }


            /// <summary>
            /// 促销生效必要条件
            /// </summary>
            [ProtoMember(23)]
            public virtual IList<Rule> Rules { get; set; }

            /// <summary>
            /// 判断限购是否有效
            /// </summary>
            /// <returns></returns>
            public bool IsValid()
            {
                var now = DateTime.Now;
                return Status && StartedOn < now && now < StoppedOn && Rules.Any(x => x.Status);
            }

            ///<summary>
            /// CartRule
            ///created on 2016-05-22 18:00:01
            ///<summary>
            [ProtoContract]
            public class Rule : IEntity<int>
            {

                ///<summary>
                /// Id
                ///<summary>
                [ProtoMember(1)]
                public int Id { get; set; }

                ///<summary>
                /// 购物车促销Id
                ///<summary>
                [ProtoMember(2)]
                public int CartId { get; set; }

                ///<summary>
                /// 开始时间
                ///<summary>
                [ProtoMember(3)]
                public DateTime StartedOn { get; set; }

                ///<summary>
                /// 结束时间
                ///<summary>
                [ProtoMember(4)]
                public DateTime StoppedOn { get; set; }

                ///<summary>
                /// 活动适用平台
                ///<summary>
                [ProtoMember(5)]
                public Plateform Plateform { get; set; }

                ///<summary>
                /// 计算方式
                ///<summary>
                [ProtoMember(6)]
                public Calc Calc { get; set; }

                ///<summary>
                /// 参数值
                ///<summary>
                [ProtoMember(7)]
                public string Value { get; set; }

                ///<summary>
                /// 优先顺序 从大到小
                ///<summary>
                [ProtoMember(8)]
                public int Priority { get; set; }

                ///<summary>
                /// 卖家Id
                ///<summary>
                [ProtoMember(9)]
                public int SellerId { get; set; }

                ///<summary>
                /// 状态 1为可用
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
                /// 通过Cale与Value生成的说明
                /// </summary>
                public string Description
                {
                    get
                    {
                        var s = string.Empty;
                        if (Calc == Calc.A)
                        {
                            var v = AsValueA();
                            if (v.IsValid)
                            {
                                if (v.And)
                                {
                                    var tpl = @"{0} -- {1} 期间有效订单总数 {2} {3}，且有效订单总金额 {4} {5}。";
                                    s = string.Format(tpl, StartedOn.ToString("yyyy-MM-dd"), StoppedOn.ToString("yyyy-MM-dd"),
                                        v.CountOpt, v.CountParam, v.AmountOpt, v.AmountParam);
                                    return s;
                                }
                                else
                                {
                                    var tpl = @"{0} -- {1} 期间有效订单总数 {2} {3}，或者有效订单总金额 {4} {5}。";
                                    s = string.Format(tpl, StartedOn.ToString("yyyy-MM-dd"), StoppedOn.ToString("yyyy-MM-dd"),
                                        v.CountOpt, v.CountParam, v.AmountOpt, v.AmountParam);
                                    return s;
                                }
                            }
                            return "参数有误。";
                        }
                        if (Calc == Calc.B)
                        {
                            var v = AsValueB();
                            if (v.IsValid)
                            {
                                if (v.And)
                                {
                                    var tpl = @"{0} -- {1} 期间已购买过 {2} 商品";
                                    s = string.Format(tpl, StartedOn.ToString("yyyy-MM-dd"), StoppedOn.ToString("yyyy-MM-dd"), string.Join(",", v.Param));
                                    return s;
                                }
                                else
                                {
                                    var tpl = @"{0} -- {1} 期间已购买过 {2} 商品中的任何一个。";
                                    s = string.Format(tpl, StartedOn.ToString("yyyy-MM-dd"), StoppedOn.ToString("yyyy-MM-dd"), string.Join(",", v.Param));
                                    return s;
                                }
                            }
                            return "参数有误。";
                        }
                        if (Calc == Calc.C)
                        {
                            var v = AsValueB();
                            if (v.IsValid)
                            {
                                if (v.And)
                                {
                                    var tpl = @"购物车中包含 {0} 商品";
                                    s = string.Format(tpl, string.Join(",", v.Param));
                                    return s;
                                }
                                else
                                {
                                    var tpl = @"购物车中包含 {0} 商品中的任何一个。";
                                    s = string.Format(tpl, string.Join(",", v.Param));
                                    return s;
                                }
                            }
                            return "参数有误。";
                        }
                        if (Calc == Calc.D)
                        {
                            var v = AsValueC();
                            if (v.IsValid)
                            {
                                var tpl = @"{0} -- {1} 期间有效评论数 {2} {3}。";
                                s = string.Format(tpl, StartedOn.ToString("yyyy-MM-dd"), StoppedOn.ToString("yyyy-MM-dd"), v.Opt, v.Param);
                                return s;
                            }
                            return "参数有误。";
                        }
                        if (Calc == Calc.E)
                        {
                            var v = AsValueC();
                            if (v.IsValid)
                            {
                                var tpl = @"{0} -- {1} 期间有效邀请人数 {2} {3}。";
                                s = string.Format(tpl, StartedOn.ToString("yyyy-MM-dd"), StoppedOn.ToString("yyyy-MM-dd"), v.Opt, v.Param);
                                return s;
                            }
                            return "参数有误。";
                        }
                        return s;
                    }
                }

                /// <summary>
                /// Calc.A
                /// </summary>
                /// <returns></returns>
                public ValueA AsValueA()
                {
                    // value值为 or:123,789 或 and:123,789
                    if (Calc != Calc.A) return new ValueA(false);
                    if (string.IsNullOrEmpty(Value)) return new ValueA(false);
                    var value = Value.ToLower();
                    if (value.StartsWith("and") || value.StartsWith("or"))
                    {
                        try
                        {
                            var o = new ValueA(true);
                            var a = value.Split(':')[0];
                            if (a == "and")
                            {
                                o.And = true;
                            }
                            var tmp = value.Split(':')[1];
                            var vals = tmp.Split(',');
                            var status = false;
                            foreach (var val in vals)
                            {
                                if (val.StartsWith("count"))
                                {
                                    status = true;
                                    o.CountOpt = val.Substring(5, 1)[0];
                                    o.CountParam = Convert.ToInt32(val.Substring(6));
                                }
                                else if (val.StartsWith("amount"))
                                {
                                    status = true;
                                    o.AmountOpt = val.Substring(6, 1)[0];
                                    o.AmountParam = Convert.ToInt32(val.Substring(7));
                                }
                            }
                            return status ? o : new ValueA(false);
                        }
                        catch (Exception)
                        {
                            return new ValueA(false);
                        }
                    }
                    return new ValueA(false);
                }

                /// <summary>
                /// Calc.B Calc.C
                /// </summary>
                /// <returns></returns>
                public ValueB AsValueB()
                {
                    // value值为 or:123,789 或 and:123,789
                    if (Calc != Calc.B && Calc != Calc.B) return new ValueB(false);
                    if (string.IsNullOrEmpty(Value)) return new ValueB(false);
                    var value = Value.ToLower();
                    var tmp = value.Split(':');
                    var op = tmp[0];
                    if (op == "and" || op == "or")
                    {
                        if (!string.IsNullOrEmpty(tmp[0]))
                        {
                            try
                            {
                                var o = new ValueB(true);
                                o.And = op == "and";
                                var v = tmp[1].Split(',');
                                o.Param = Array.ConvertAll(v, int.Parse);
                                return o;
                            }
                            catch (Exception)
                            {
                                return new ValueB(false);
                            }
                        }
                    }
                    return new ValueB(false);
                }

                /// <summary>
                /// 适用于Calc.D Calc.E
                /// </summary>
                /// <returns></returns>
                public ValueC AsValueC()
                {
                    // value值为 >123 或 =123 或 >123
                    if (Calc != Calc.D && Calc != Calc.E) return new ValueC(false);
                    if (string.IsNullOrEmpty(Value)) return new ValueC(false);
                    if (Value[0] != '<' && Value[0] != '=' && Value[0] != '>') return new ValueC(false);
                    try
                    {
                        return new ValueC(true) { Opt = Value[0], Param = Convert.ToInt32(Value.Substring(1)) };
                    }
                    catch (Exception)
                    {
                        return new ValueC(false);
                    }
                }

                /// <summary>
                /// 适用于Calc.A
                /// </summary>
                public class ValueA
                {
                    /// <summary>
                    /// 是否订单数/金额都必须满足
                    /// </summary>
                    public bool And { get; set; }
                    /// <summary>
                    /// 订单数比较值
                    /// </summary>
                    public char CountOpt { get; set; }
                    /// <summary>
                    /// 订单数
                    /// </summary>
                    public int CountParam { get; set; }
                    /// <summary>
                    /// 订单金额比较值
                    /// </summary>
                    public char AmountOpt { get; set; }
                    /// <summary>
                    /// 订单金额
                    /// </summary>
                    public int AmountParam { get; set; }
                    /// <summary>
                    /// 是否有效
                    /// </summary>
                    public bool IsValid { get; private set; }

                    public ValueA(bool isValid)
                    {
                        CountOpt = '0';
                        AmountOpt = '0';
                        IsValid = isValid;
                    }
                }

                /// <summary>
                /// 适用于Calc.B Calc.C
                /// </summary>
                public class ValueB
                {
                    /// <summary>
                    /// 是否商品都必须满足
                    /// </summary>
                    public bool And { get; set; }
                    /// <summary>
                    /// 商品Id
                    /// </summary>
                    public int[] Param { get; set; }
                    /// <summary>
                    /// 是否有效
                    /// </summary>
                    public bool IsValid { get; private set; }
                    public ValueB(bool isValid)
                    {
                        IsValid = isValid;
                    }
                }

                /// <summary>
                /// 适用于Calc.D Calc.E
                /// </summary>
                public class ValueC
                {
                    /// <summary>
                    /// 比较符
                    /// </summary>
                    public char Opt { get; set; }
                    /// <summary>
                    /// 比较值
                    /// </summary>
                    public int Param { get; set; }
                    /// <summary>
                    /// 是否有效
                    /// </summary>
                    public bool IsValid { get; private set; }
                    public ValueC(bool isValid)
                    {
                        Opt = '0';
                        IsValid = isValid;
                    }
                }
            }
        }
    }
}

