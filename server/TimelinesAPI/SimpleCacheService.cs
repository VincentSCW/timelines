using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimelinesAPI
{
    public class SimpleCacheService<T>
    {
        private readonly Dictionary<string, T> _cache;

        public SimpleCacheService()
        {
            _cache = new Dictionary<string, T>();
        }

        private bool IsInCachce(string key)
        {
            return _cache.ContainsKey(key);
        }

        public bool TryGetFromCache(string key, out T value)
        {
            if (IsInCachce(key))
            {
                value = _cache[key];
                return true;
            }

            value = default(T);
            return false;
        }

        public void StoreInCache(string key, T value)
        {
            if (!IsInCachce(key))
                _cache.Add(key, value);
            else
            {
                _cache[key] = value;
            }
        }

        public void RemoveFromCache(string key)
        {
            if (IsInCachce(key))
                _cache.Remove(key);
        }
    }
}
