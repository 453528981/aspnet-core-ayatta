namespace Ayatta.Domain
{
    public partial class User
    {
        /// <summary>
        /// User.Invoice
        /// </summary>
        public class Invoice : BaseEntity<int>
        {
            public int UserId { get; set; }
            public string Group { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public bool IsDefault { get; set; }
        }
    }
}