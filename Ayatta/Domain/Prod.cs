namespace Ayatta.Domain
{
    public static partial class Prod
    {
        public struct Prop
        {
            public int PId;
            public string PName;
            public int VId;
            public string VName;
            public string Extra;
        }

        /// <summary>
        ///  商品状态
        /// </summary>
        public enum Status : byte
        {
            /// <summary>
            /// 上线出售中
            /// </summary>
            Online = 0,

            /// <summary>
            /// 下线库中
            /// </summary>
            Offline = 1,

            /// <summary>
            /// 已删除
            /// </summary>
            Deleted = 2,

            /// <summary>
            /// 隔离的 违规 违反广告法 包含敏感词等
            /// </summary>
            Isolated = 3,

            /// <summary>
            /// 禁用的 被品牌商投诉侵权等
            /// </summary>
            Forbidden = 4
        }

    }
}