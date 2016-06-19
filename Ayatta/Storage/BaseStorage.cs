using System;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;

namespace Ayatta.Storage
{
    public abstract class BaseStorage
    {

        private readonly StorageOptions options;

        protected IDbConnection BaseConn
        {
            get
            {
                return new MySqlConnection(options.BaseConnStr ?? "server=127.0.0.1;database=base;uid=root;pwd=root;charset=utf8");
            }
        }

        protected IDbConnection StoreConn
        {
            get
            {
                return new MySqlConnection(options.StoreConnStr ?? "server=127.0.0.1;database=store;uid=root;pwd=root;charset=utf8");
            }
        }
        protected IDbConnection PromotionConn
        {
            get
            {
                return new MySqlConnection(options.StoreConnStr ?? "server=127.0.0.1;database=promotion;uid=root;pwd=root;charset=utf8");
            }
        }
        protected BaseStorage(IOptions<StorageOptions> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            options = optionsAccessor.Value;
        }

    }
}