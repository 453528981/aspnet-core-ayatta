using System;

namespace Ayatta.Domain
{
    /// <summary>
    /// User.OAuth
    /// </summary>
    public partial class User
    {
        #region User.OAuth

        public class OAuth : BaseEntity<int>
        {
            /// <summary>
            /// Guid
            /// </summary>
            public string Guid { get; set; }

            /// <summary>
            /// OpenId
            /// </summary>
            public string OpenId { get; set; }

            /// <summary>
            /// Name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Scope
            /// </summary>
            public string Scope { get; set; }

            /// <summary>
            /// AccessToken
            /// </summary>
            public string AccessToken { get; set; }

            /// <summary>
            /// RefreshToken
            /// </summary>
            public string RefreshToken { get; set; }

            /// <summary>
            /// ExpirationTime
            /// </summary>
            public DateTime? ExpirationTime { get; set; }

            /// <summary>
            /// Extra
            /// </summary>
            public string Extra { get; set; }

        }

        #endregion
    }
}