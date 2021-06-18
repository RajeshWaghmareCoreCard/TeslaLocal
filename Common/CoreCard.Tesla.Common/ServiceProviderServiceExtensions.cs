using System;

namespace CoreCard.Tesla.Common
{
    public static class ServiceProviderServiceExtensions
    {
        public static T GetServiceByType<T>(this IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }
    }
}