using System;
using ProtoBuf;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Options;
namespace Ayatta.LocalCache
{
    public abstract class BaseLocalCache<T> : IDisposable where T : class, new()
    {
        protected T Data { get; private set; }
        public DateTime NextSyncTime { get; private set; }
        public DateTime NextSaveTime { get; private set; }

        private bool saving;
        private readonly Timer timer;
        protected readonly LocalCacheOptions options;

        protected BaseLocalCache(IOptions<LocalCacheOptions> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            options = optionsAccessor.Value;
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (options != null)
            {
                Data = new T();
                var exists = false;
                DateTime? time = null;

                if (File.Exists(options.CacheFile))
                {
                    exists = true;
                    using (var file = System.IO.File.OpenRead(options.CacheFile))
                    {
                        Data = Serializer.Deserialize<T>(file);
                    }
                    time = File.GetLastWriteTime(options.CacheFile);
                }

                Sync(time);
                if (!exists)
                {
                    Save();
                }

                if (options.SyncEnable)
                {
                    timer = new Timer(Callback, null, options.SyncInterval, 1000);
                }
            }
        }

        private void Callback(object state)
        {
            if (options.SyncEnable && timer != null)
            {
                var now = DateTime.Now;
                var syncTime = NextSyncTime;
                var saveTime = NextSaveTime;
                if (syncTime != null)
                {
                    if (now.ToString("yyyy-MM-dd HH:mm:ss") == syncTime.ToString("yyyy-MM-dd HH:mm:ss"))
                    {
                        Sync(now);
                    }
                }
                if (saveTime != null)
                {
                    if (now.ToString("yyyy-MM-dd HH:mm:ss") == saveTime.ToString("yyyy-MM-dd HH:mm:ss"))
                    {
                        Save();
                    }
                }
            }
        }

        private void Sync(DateTime? time)
        {
            if (saving)
            {
                return;
            }
            var data = Read(time);
            if (!time.HasValue)
            {
                Data = data;
                NextSyncTime = DateTime.Now.AddSeconds(options.SyncInterval);
                return;
            }
            ReadCallback(data);

            NextSyncTime = DateTime.Now.AddSeconds(options.SyncInterval);
        }

        private void Save()
        {
            if (saving)
            {
                return;
            }
            saving = true;
            using (var file = File.Create(options.CacheFile))
            {
                Serializer.Serialize(file, Data);
            }
            saving = false;

            NextSaveTime = DateTime.Now.AddSeconds(options.SaveInterval);
        }

        protected abstract T Read(DateTime? time);

        protected abstract void ReadCallback(T data);

        public virtual void Dispose()
        {
            if (timer != null)
            {
                timer.Dispose();
            }
            Data = null;
        }
    }
}