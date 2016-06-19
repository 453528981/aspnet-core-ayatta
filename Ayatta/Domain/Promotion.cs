
using System;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    /// <summary>
    /// 促销 http://bbs.taobao.com/catalog/thread/16543510-264265243.htm
    /// </summary>
    public static partial class Promotion
    {
        /// <summary>
        /// 促销类型
        /// </summary>
        public enum Catg : sbyte
        {
            None = 0,
            
            /// <summary>
            /// 特价
            /// </summary>            
            A,
            
            /// <summary>
            /// 店铺优惠
            /// </summary>
            B,
            
            /// <summary>
            /// 搭配组合套餐
            /// </summary>
            C,
            
            /// <summary>
            /// 购物车促销
            /// </summary>
            D
        }

        /// <summary>
        /// 促销活动用户参与限制
        /// </summary>
        public enum LimitBy : sbyte
        {
            /// <summary>
            /// 无限制
            /// </summary>

            None = 0,

            /// <summary>
            /// N次
            /// </summary>
            NTimesOnly = 1,

            /// <summary>
            /// 每用户N次
            /// </summary>
            NTimesPerUser = 2
        }

        /// <summary>
        /// 特价类型 A打折  B减价  C促销价
        /// </summary>
        public enum SpecialPriceCatg : sbyte
        {
            /// <summary>
            /// 打折
            /// </summary>
            A = 1,

            /// <summary>
            /// 减价
            /// </summary>
            B = 2,

            /// <summary>
            /// 促销价
            /// </summary>
            C = 3
        }

        /// <summary>
        /// 适用于 作用于
        /// </summary>
        public enum ApplyTo : sbyte
        {
            /// <summary>
            /// 默认值
            /// </summary>
            None = 0,

            /// <summary>
            /// 订单总金额 (商品总金额+运费)
            /// </summary>
            OrderTotal = 1,

            /// <summary>
            /// 商品总金额
            /// </summary>
            OrderTotalSub = 2,

            /// <summary>
            /// 运费
            /// </summary>
            Freight = 3,

            /// <summary>
            /// 商品类目
            /// </summary>
            ProdCatetory = 4,

            /// <summary>
            /// 商品品牌
            /// </summary>
            ProdBrand = 5,

            /// <summary>
            /// 商品Id
            /// </summary>
            ProdItem = 6,

            /// <summary>
            /// 配送区域
            /// </summary>
            Area = 7,

            /// <summary>
            /// 用户
            /// </summary>
            User = 8

        }

        /// <summary>
        /// 计算方式
        /// </summary>
        public enum Calc : sbyte
        {
            /// <summary>
            /// 订单总量/金额
            /// </summary>
            A,
            /// <summary>
            /// 已购买过
            /// </summary>
            B,
            /// <summary>
            /// 购物车中包含
            /// </summary>
            C,
            /// <summary>
            /// 有效评论/晒单
            /// </summary>
            D,
            /// <summary>
            /// 有效邀请人
            /// </summary>
            E
        }
    }
}