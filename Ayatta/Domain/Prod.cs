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
            //public int ParentPId;     
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
            /// 库中
            /// </summary>
            Offline = 1,

            /// <summary>
            /// 删除
            /// </summary>
            Deleted = 2
        }

    }
}