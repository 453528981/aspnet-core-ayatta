using System.Collections.Generic;

namespace Ayatta.Domain
{
    public class Help : BaseEntity<int>
    {
        /// <summary>
        /// 分组
        /// </summary>
        public byte Grpup { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 外部链接
        /// </summary>
        public string ExternalUri { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 是否显示在侧栏菜单
        /// </summary>
        public bool ShowOnMenu { get; set; }

        public static IDictionary<byte, string> GrpupDic
        {
            get
            {
                var dic = new Dictionary<byte, string>();
                dic.Add(1, "购物指南");
                return dic;
            }
        }
    }
}