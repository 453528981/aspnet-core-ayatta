namespace Ayatta.LocalCache
{  
    public class LocalCacheOptions
    {
        public bool SyncEnable{get;set;}
        public int SyncInterval{get;set;}
        public int SaveInterval{get;set;}
        public string CacheFile{get;set;}
        public string ConnectionString{get;set;}	
    }
}