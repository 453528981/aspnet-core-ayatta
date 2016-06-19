using System;
using ProtoBuf;
using System.IO;
using Microsoft.Extensions.Caching.Distributed;

namespace Microsoft.Extensions.Caching.Redis
{
    public static class RedisCacheExtensions
    {
        public static T Put<T>(this RedisCache cache, string cacheKey, Func<T> sourceGetter, DateTime absoluteExpiration) where T : class
        {
            return Get(
                (out T cacheData) =>
                {
                    cacheData = cache.Get<T>(cacheKey);
                    return cacheData != null;
                },
                sourceGetter,
                cacheData => cache.Set(cacheKey, cacheData, absoluteExpiration));
        }

        private delegate bool DataGetter<T>(out T data);

        private static T Get<T>(DataGetter<T> dataGetter, Func<T> sourceGetter, Action<T> dataSetter)
        {
            T data;
            if (dataGetter(out data))
            {
                return data;
            }
            data = sourceGetter();
            if (data != null)
            {
                dataSetter(data);
            }
            return data;
        }


        public static T Get<T>(this RedisCache cache, string key)
        {
            var data = cache.Get(key);
            using (var stream = new MemoryStream(data))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }

        public static void Set<T>(this RedisCache cache, string key, T data, DateTime absoluteExpiration)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, data);
                cache.Set(key, stream.ToArray(), new DistributedCacheEntryOptions() { AbsoluteExpiration = absoluteExpiration });
            }

        }
    }
}
