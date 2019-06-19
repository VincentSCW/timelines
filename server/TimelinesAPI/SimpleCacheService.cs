using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimelinesAPI
{
    public class SimpleCacheService<T>
    {
        private readonly ConcurrentDictionary<string, T> _cache;
        private readonly object _syncObj;

        public SimpleCacheService()
        {
            _cache = new ConcurrentDictionary<string, T>();
            _syncObj = new object();
        }

        public bool TryGetFromCache(string key, out T value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public void StoreInCache(string key, T value)
        {
            _cache.AddOrUpdate(key, value, (k, v) => value);
        }

        public void RemoveFromCache(string key)
        {
            _ = _cache.TryRemove(key, out _);
        }
    }
}
