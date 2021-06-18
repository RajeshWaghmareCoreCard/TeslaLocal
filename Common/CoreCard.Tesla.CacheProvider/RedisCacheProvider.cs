using StackExchange.Redis;
using CoreCard.Tesla.Common;
using System.Threading.Tasks;
using System;

namespace CoreCard.Telsa.Cache
{
    public class RedisCacheProvider : ICacheProvider
    {
        private readonly string connectionString;
        private ConnectionMultiplexer ConnectionMultiplexer;
        ISerializer serializer;
        public RedisCacheProvider(string connectionString, ISerializer serializer)
        {
            this.connectionString = connectionString;
            this.serializer = serializer;
            RegisterRedis();
        }
        void RegisterRedis()
        {
            ConnectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
        }
        public async Task<bool> SetValueAsync<T>(string key, T value)
        {
            var finalKey = $"{typeof(T).Name}{key}";
            var serializedObject = serializer.Serialize(value);
            var connection = ConnectionMultiplexer.GetDatabase();
            return await connection.SetAddAsync(finalKey, serializedObject);
        }
        public async Task<T> GetValueAsync<T>(string key)
        {
            var connection = ConnectionMultiplexer.GetDatabase();
            try
            {
                var finalKey = $"{typeof(T).Name}{key}";
                var byteArray = await connection.StringGetAsync(finalKey);
                return serializer.Deserialize<T>(byteArray);
            }
            catch
            {
            }
            return default;
        }
        public async Task<bool> RemoveKeyAsync(string key)
        {
            var connection = ConnectionMultiplexer.GetDatabase();
            try
            {
                return await connection.KeyDeleteAsync(key);
            }
            catch
            {
            }
            return false;
        }
        public async Task<bool> SetRemoveAsync<T>(string key, T item)
        {
            try
            {
                var finalKey = $"{typeof(T).Name}{key}";
                var connection = ConnectionMultiplexer.GetDatabase();
                var serializedObject = serializer.Serialize(item);
                return await connection.SetRemoveAsync(finalKey, serializedObject);
            }
            catch
            {

            }
            return false;
        }

        public async Task<bool> SetValueWithExpirationAsync<T>(string key, T value, DateTime expirationTime)
        {
            var finalKey = $"{typeof(T).Name}{key}";
            var serializedObject = serializer.Serialize(value);
            var connection = ConnectionMultiplexer.GetDatabase();
            await connection.SetAddAsync(key, serializedObject);
            connection.KeyExpire(finalKey, expirationTime);
            return true;
        }

        public async Task<long> StringIncrementAsync<T>(string key, DateTime expirationTime)
        {
            var finalKey = $"{key}";
            var connection = ConnectionMultiplexer.GetDatabase();
            var incrementalValue = await connection.StringIncrementAsync(finalKey);
            connection.KeyExpire(finalKey, expirationTime);
            return incrementalValue;
        }

        public async Task<long> IncrementObjectAsync<T>(string key, T value, DateTime expirationTime)
        {
            var finalKeyString = $"{typeof(T).Name}{key}-String";
            var incrementalValue = await StringIncrementAsync<T>(key, expirationTime);
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

//Register Redis cache
// var options = new ConfigurationOptions
// {
//     EndPoints = { "hostname:port" },
//     Password = "password",
//     Ssl = true
// };
// options.CertificateSelection += delegate
// {
//     return new X509Certificate2(@"d:\path\filname.pfx", "");
// };
// services.AddSingleton<ConnectionMultiplexer>(ConnectionMultiplexer.Connect(Configuration["ConnectionString:CacheConnection"]));
// services.AddSingleton<IDatabase>(ConnectionMultiplexer.Connect(Configuration["ConnectionString:CacheConnection"]).GetDatabase());