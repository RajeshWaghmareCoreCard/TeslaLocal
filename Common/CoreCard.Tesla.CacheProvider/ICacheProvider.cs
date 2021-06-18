using System;
using System.Threading.Tasks;

namespace CoreCard.Telsa.Cache
{
    public interface ICacheProvider
    {
        Task<bool> SetValueAsync<T>(string key, T value);
        Task<T> GetValueAsync<T>(string key);
        Task<bool> RemoveKeyAsync(string key);
        Task<bool> SetRemoveAsync<T>(string key, T item);
        Task<bool> SetValueWithExpirationAsync<T>(string key, T value, DateTime expirationTime);
        Task<long> StringIncrementAsync<T>(string key, DateTime expirationTime);
        Task<long> IncrementObjectAsync<T>(string key, T value, DateTime expirationTime);
        Task<Tuple<bool, T>> IsExist<T>(string key);
    }
}