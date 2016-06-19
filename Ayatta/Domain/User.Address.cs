namespace Ayatta.Domain
{
    public partial class User
    {
        /// <summary>
        /// User.Address
        /// </summary>
        public class Address : BaseEntity<int>
        {
            public int UserId { get; set; }
            public string Name { get; set; }
            public string Company { get; set; }
            public Enum.AddressGroup Group { get; set; }
            public int Country { get; set; }
            public string AreaId { get; set; }
            public string Province { get; set; }
            public string City { get; set; }
            public string District { get; set; }
            public string Street { get; set; }
            public string PostalCode { get; set; }
            public string Phone { get; set; }
            public string Mobile { get; set; }
            public bool IsDefault { get; set; }

            //public static Func<string, IList<Area>, string> GetAreaName { get; set; }

            public override bool Equals(object obj)
            {
                var target = obj as Address;
                if (target != null)
                {
                    return (Name == target.Name && Country == target.Country && AreaId == target.AreaId &&
                            Street == target.Street && PostalCode == target.PostalCode &&
                            Phone == target.Phone && Mobile == target.Mobile);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Id;
            }

        }
    }
}