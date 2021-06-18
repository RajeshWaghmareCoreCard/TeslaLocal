using System;
using System.IO;
using CoreCard.Telsa.Cache;
using CoreCard.Tesla.CacheProvider;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Falcon.Adc;
using CoreCard.Tesla.Falcon.Adc.Contracts;
using CoreCard.Tesla.Falcon.ADC;
using CoreCard.Tesla.Falcon.ADC.Contracts;
using CoreCard.Tesla.Falcon.DataModels;
using CoreCard.Tesla.Falcon.DataModels.Common;
using CoreCard.Tesla.Falcon.DbRepository;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;
using CoreCard.Tesla.Falcon.ServiceInterfaces;
using CoreCard.Tesla.Falcon.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Tesla.TokenizationProvider;
namespace CoreCard.Tesla.Falcon
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterFalconServices(this IServiceCollection services)
        {
            services.AddTransient<IPurchaseService, PurchaseService>();
            services.AddTransient<Lazy<IPurchaseService>>(c => new Lazy<IPurchaseService>(c.GetService<IPurchaseService>));
        }

        public static void RegisterAdcs(this IServiceCollection services)
        {
            services.AddTransient<ICardNumberAdc, CardNumberAdc>();
            services.AddTransient<Lazy<ICardNumberAdc>>(c => new Lazy<ICardNumberAdc>(c.GetService<ICardNumberAdc>));

            services.AddTransient<ICardExpirationAdc, CardExpirationAdc>();
            services.AddTransient<Lazy<ICardExpirationAdc>>(c => new Lazy<ICardExpirationAdc>(c.GetService<ICardExpirationAdc>));

            services.AddTransient<IAuthDecisionControlResolver, AuthDecisionControlResolver>();
            services.AddTransient<IAuthDecisionControlProvider, AuthDecisionControlProvider>();
        }
        internal static void RegisterDatabase(this IServiceCollection services, string connectionString)
        {
            // var connStringBuilder = new NpgsqlConnectionStringBuilder() { IncludeErrorDetails = true };
            // connStringBuilder.Host = "localhost";
            // connStringBuilder.Port = 26257;6
            // connStringBuilder.SslMode = SslMode.Disable;
            // connStringBuilder.Username = "root";
            // connStringBuilder.Database = "falcon";
            services.AddTransient<IDatabaseConnectionResolver, DatabaseConnectionResolver>(x => new DatabaseConnectionResolver(connectionString));
        }
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICustomerAccountRepository, CustomerAccountRepository>();
            services.AddTransient<ICardRepository, CardRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IAdcRepository, AdcRepository>();
            services.AddTransient<ICustomerCardAccountUnit, CustomerCardAccountUnit>();
            services.AddTransient<IProductAdcRepository, ProductAdcRepository>();
            services.AddTransient<IAccountAdcRepository, AccountAdcRepository>();
            services.AddTransient<ICardAdcRepository, CardAdcRepository>();
            services.AddTransient<IAdcUnitOfWork, AdcUnitOfWork>();
        }

        public static void RegisterRequestExtensions(this IServiceCollection services)
        {
            services.AddTransient<PurchaseRequestExt, PurchaseRequestExt>();
        }

        public static void RegisterCache(this IServiceCollection services, string cacheType, string connectionString)
        {
            if (cacheType == "Memory")
            {
                services.AddSingleton<ICacheProvider>(new MemoryCacheProvider(new NewtonsoftSerializer()));
            }
            else
            {
                services.AddSingleton<ICacheProvider>(new RedisCacheProvider(connectionString, new NewtonsoftSerializer()));
            }
        }

        public static void RegisterTokenizationUtilChannel(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSingleton<ITokenizationUtilityChannel>(sp =>
           {
               return new TokenizationUtilityChannel(File.ReadAllText("Falcon_RSA_private_key.pem"), Configuration["FalconAppSetting:AesRotationMinutes"].TryToDouble(), Configuration["FalconAppSetting:ModuleKeyId"], Configuration["FalconAppSetting:TokenizationURL"], sp.GetService<ICacheProvider>(), Configuration["FalconAppSetting:TokenizationModuleName"]);
           });
        }
        public static void RegisterConfigs(this IServiceCollection services, IConfiguration Configuration)
        {
            IConfigurationSection falconConfiguration = Configuration.GetSection("FalconAppSetting");
            services.Configure<FalconAppSetting>(falconConfiguration);
        }
        public static void RegisterHttpClients(this IServiceCollection services, IConfiguration Configuration)
        {
            // Create the retry policy we want
            var retryPolicy = HttpPolicyExtensions
                            .HandleTransientHttpError() // HttpRequestException, 5XX and 408
                            .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(1000));

            services.AddHttpClient("TokenizationModule", c =>
            {
                c.BaseAddress = new Uri(Configuration["FalconAppSetting:TokenizationURL"]);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("User-Agent", Configuration["FalconAppSetting:TokenizationModuleName"]);
                c.Timeout = TimeSpan.FromMilliseconds(1000);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(retryPolicy);
        }
        internal static void InitApplicationCache(this IServiceCollection services)
        {
            services.AddSingleton<IFalconCacheService, FalconCacheService>();
        }
    }
}