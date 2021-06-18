using System;
using System.Threading.Tasks;
using CoreCard.Telsa.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace CoreCard.Tesla.CacheProvider
{
    public class ExpiryObject<T>
    {
        public long Counter { get; set; }
    }
    public class MemoryCacheProvider : ICacheProvider
    {
        private readonly ISerializer serializer;

        public MemoryCache Cache { get; set; }
        public MemoryCacheProvider(ISerializer serializer)
        {
            RegisterMemoryCache();
            this.serializer = serializer;
        }
        void RegisterMemoryCache()
        {
            Cache = new MemoryCache(new MemoryCacheOptions());

        }

        public async Task<bool> SetValueAsync<T>(string key, T value)
        {
            try
            {
                Cache.Set<T>(key, value);
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> GetValueAsync<T>(string key)
        {
            try
            {
                Cache.TryGetValue<T>(key, out T result);
                return await Task.FromResult(result);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> RemoveKeyAsync(string key)
        {
            try
            {
                Cache.Remove(key);
                return await Task.FromResult(true);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SetRemoveAsync<T>(string key, T item)
        {
            try
            {
                Cache.Remove(key);
                return await Task.FromResult(true);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SetValueWithExpirationAsync<T>(string key, T value, DateTime expirationTime)
        {
            try
            {
                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions();
                cacheEntryOptions.AbsoluteExpiration = expirationTime;

                Cache.Set<T>(key, value, cacheEntryOptions);
                return await Task.FromResult(true);
            }
            catch
            {
                throw;
            }
        }

        public async Task<long> StringIncrementAsync<T>(string key, DateTime expirationTime)
        {
            //TODO - not tested
            var existingObject = Cache.Get<ExpiryObject<string>>(key);
            if (existingObject != null)
            {
                existingObject = new ExpiryObject<string>();
            }
            existingObject.Counter += 1;
            // Set cache options.
            var cacheEntryOptions = new MemoryCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = expirationTime;
            Cache.Set(key, existingObject, cacheEntryOptions);
            return await Task.FromResult(existingObject.Counter);
        }

        public async Task<long> IncrementObjectAsync<T>(string key, T value, DateTime expirationTime)
        {
            //TODO - not tested
            var finalKeyString = $"{typeof(T).Name}{key}-String";
            var incrementalValue = await StringIncrementAsync<T>(finalKeyString, expirationTime);
            await SetValueWithExpirationAsync(finalKeyString, value, expirationTime);
            return incrementalValue;
        }

         public async Task<Tuple<bool, T>> IsExist<T>(string key)
        {
            var existingValue = await GetValueAsync<T>(key);
            if (existingValue != null)
            {
                return new Tuple<bool, T>(true, existingValue);
            }
            else
            {
                return new Tuple<bool, T>(false, existingValue);
            }
        }
    }
}
