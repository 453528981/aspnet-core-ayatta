using System;

namespace Ayatta.Domain
{
    public partial class User
    {
        public static class Enum
        {
            /// <summary>
            /// 用户角色
            /// </summary>
            [Flags]
            public enum Role : byte
            {
                /// <summary>
                /// None
                /// </summary>
                None = 0,
                /// <summary>
                /// 买家
                /// </summary>
                Buyer = 1,
                /// <summary>
                /// 卖家
                /// </summary>
                Seller = 2,
                /// <summary>
                /// 管理员
                /// </summary>
                Administrator = 4
            }

            /// <summary>
            /// 用户级别
            /// </summary>
            public enum Level : byte
            {
                /// <summary>
                /// None
                /// </summary>
                None = 0,
                /// <summary>
                /// 一级
                /// </summary>
                One = 1,
                /// <summary>
                /// 二级
                /// </summary>
                Two = 2
            }

            /// <summary>
            /// 商家特殊许可
            /// </summary>
            [Flags]
            public enum Permit : byte
            {
                /// <summary>
                /// None
                /// </summary>
                None = 0,
                /// <summary>
                /// 竞拍
                /// </summary>
                Auction = 1
            }

            /// <summary>
            /// 用户限制
            /// </summary>
            [Flags]
            public enum Limit : byte
            {
                /// <summary>
                /// 无限制
                /// </summary>
                None = 0
            }

            /// <summary>
            /// 用户状态
            /// </summary>
            public enum Status : byte
            {
                Normal = 0,
                Deleted = 255
            }

            /// <summary>
            /// 用户性别
            /// </summary>
            public enum Gender : byte
            {
                /// <summary>
                /// 保密
                /// </summary>

                Secrect = 0,

                /// <summary>
                /// 男
                /// </summary>
                Male = 1,

                /// <summary>
                /// 女
                /// </summary>
                Female = 2
            }

            /// <summary>
            /// 用户婚姻状况
            /// </summary>
            public enum Marital : byte
            {
                /// <summary>
                /// 保密
                /// </summary>
                Secrect = 0,

                /// <summary>
                /// 单身
                /// </summary>
                Single = 1,

                /// <summary>
                /// 已婚
                /// </summary>
                Married = 2
            }

            /// <summary>
            /// 用户收/发/退货地址
            /// </summary>
            public enum AddressGroup : byte
            {
                /// <summary>
                /// 收货地址
                /// </summary>
                Receive = 0,
                /// <summary>
                /// 发货地址
                /// </summary>
                Send = 1,
                /// <summary>
                /// 退货地址
                /// </summary>
                Refund = 2
            }
        }

    }
}