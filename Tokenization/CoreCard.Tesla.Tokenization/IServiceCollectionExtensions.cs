using System;
using CoreCard.Telsa.Cache;
using CoreCard.Tesla.CacheProvider;
using CoreCard.Tesla.Common;
using CoreCard.Tesla.Tokenization.DataModels;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;
using CoreCard.Tesla.Tokenization.DataModels.NCipher;
using CoreCard.Tesla.Tokenization.DataModels.Repository;
using CoreCard.Tesla.Tokenization.Repository;
using CoreCard.Tesla.Tokenization.Services;
using CoreCard.Tesla.Tokenization.TokenFamilyProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace CoreCard.Tesla.Tokenization
{
    public static class IServiceCollectionExtensions
    {
        internal static void RegisterConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection tokenizationConfig = configuration.GetSection("TokenizationConfig");
            services.Configure<TokenizationConfig>(tokenizationConfig);
        }

        internal static void RegisterDatabase(this IServiceCollection services, string connectionString)
        {
            // var connStringBuilder = new NpgsqlConnectionStringBuilder() { IncludeErrorDetails = true };
            // connStringBuilder.Host = "localhost";
            // connStringBuilder.Port = 26257;
            // connStringBuilder.SslMode = SslMode.Disable;
            // connStringBuilder.Username = "root";
            // connStringBuilder.Database = "falcon";
            services.AddTransient<IDatabaseConnectionResolver, DatabaseConnectionResolver>(x => new DatabaseConnectionResolver(connectionString));
        }

        internal static void RegisterCache(this IServiceCollection services, string cacheType, string connectionString)
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

        internal static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICardTokenRepository, CardTokenRepository>();
            services.AddTransient<IModuleKeyRepository, ModuleKeyRepository>();

            services.AddSingleton<INCipherSettingsRepository, NCipherSettingsRepository>();
            services.AddTransient<IModuleSessionRepository, ModuleSessionRepository>();
            services.AddTransient<IModulePermissionRepository, ModulePermissionRepository>();
            services.AddTransient<IOtherTokenFamilyRepository, OtherTokenFamilyRepository>();
            services.AddTransient<IHsmActiveKeysRepository, HsmActiveKeysRepository>();
        }

        internal static void RegisterFamilyProviders(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();

            services.AddTransient<ITokenFamilyProvider, TokenFamilyProvider>();
            services.AddTransient<ICardTokenFamilyProvider, CardTokenFamilyProvider>();
            services.AddTransient<ISSNTokenFamilyProvider, SSNTokenFamilyProvider>();
            services.AddTransient<IOtherTokenFamilyProviders, OtherTokenFamilyProviders>();

            services.AddTransient<Lazy<ICardTokenFamilyProvider>>(c => new Lazy<ICardTokenFamilyProvider>(c.GetService<ICardTokenFamilyProvider>));
            services.AddTransient<Lazy<ISSNTokenFamilyProvider>>(c => new Lazy<ISSNTokenFamilyProvider>(c.GetService<ISSNTokenFamilyProvider>));
            services.AddTransient<Lazy<IOtherTokenFamilyProviders>>(c => new Lazy<IOtherTokenFamilyProviders>(c.GetService<IOtherTokenFamilyProviders>));
            
            services.AddTransient<ITokenFamilyResolver, TokenFamilyResolver>();
        }
        internal static void RegisterHSMChannel(this IServiceCollection services)
        {

            services.AddTransient<IChannel, NCipherChannel>();
            services.AddSingleton<INCipherProvider, NCipherProvider>();
        }

        internal static void RegisterAPIs(this IServiceCollection services, string serviceId)
        {



            services.AddTransient<ICreateSessionService, CreateSessionService>();
            services.AddTransient<Lazy<ICreateSessionService>>(c => new Lazy<ICreateSessionService>(c.GetService<ICreateSessionService>));

            services.AddTransient<ITokenizationService, TokenizationService>();
            services.AddTransient<Lazy<ITokenizationService>>(c => new Lazy<ITokenizationService>(c.GetService<ITokenizationService>));

            services.AddTransient<IDetokenizationService, DetokenizationService>();
            services.AddTransient<Lazy<IDetokenizationService>>(c => new Lazy<IDetokenizationService>(c.GetService<IDetokenizationService>));

        }
    }
}