using CoreCard.Tesla.Falcon.DataModels.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tesla.TokenizationProvider;

namespace CoreCard.Tesla.Falcon
{
    public static class AppInit
    {
        public static void InitAppliationCache(this IApplicationBuilder app)
        {
            var tokenizationInit = app.ApplicationServices.GetService<ITokenizationUtilityChannel>();

            var service = app.ApplicationServices.GetService<IFalconCacheService>();
            service.Init().Wait();
        }
    }
}
