namespace Ayatta.Domain
{
    public class Area : IEntity<string>
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 上级Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分组1为省/直辖市 2为市 3为区
        /// </summary>
        public byte Group { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public int Country { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string PostalCode { get; set; }
    }
}