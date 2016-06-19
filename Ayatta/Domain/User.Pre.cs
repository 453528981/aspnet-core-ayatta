namespace Ayatta.Domain
{
    public partial class User
    {
        /// <summary>
        /// User.Pre
        /// </summary>
        public class Pre : BaseEntity<string>
        {
            public byte Origin { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            public string Browser { get; set; }
            public string UserAgent { get; set; }
            public string IpAddress { get; set; }
            public string UrlReferrer { get; set; }
            public string TraceCode { get; set; }
        }
    }
}