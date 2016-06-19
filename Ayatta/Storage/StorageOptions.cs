
using Microsoft.Extensions.Options;

namespace Ayatta.Storage
{
    /// <summary>
    /// Configuration options for <see cref="StorageOptions"/>.
    /// </summary>
    public class StorageOptions : IOptions<Ayatta.Storage.StorageOptions>
    {
        /// <summary>
        /// Base库连接字符串名
        /// </summary>
        public string BaseConnStr { get; set; }

        /// <summary>
        /// Store库连接字符串名
        /// </summary>
        public string StoreConnStr { get; set; }

        StorageOptions IOptions<StorageOptions>.Value
        {
            get { return this; }
        }
    }
}