using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCard.Telsa.Cache;
using CoreCard.Tesla.CacheProvider;
using CoreCard.Tesla.Tokenization.DataModels.CommonTypes;
using CoreCard.Tesla.Tokenization.DataModels.Interfaces;
using CoreCard.Tesla.Tokenization.DataModels.Types;
using CoreCard.Tesla.Tokenization.Repository;
using CoreCard.Tesla.Tokenization.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Npgsql;
using StackExchange.Redis;

namespace CoreCard.Tesla.Tokenization
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // this.services = services;
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoreCard.Tesla.Tokenization", Version = "v1" });
                c.OperationFilter<TokenizationCustomHeaderFilter>();
            });

            // services.AddApiVersioning(x =>
            //     {
            //         x.DefaultApiVersion = new ApiVersion(1, 0);
            //         x.AssumeDefaultVersionWhenUnspecified = true;
            //         x.ReportApiVersions = true;
            //     });

            services.RegisterConfigs(Configuration);

            services.RegisterDatabase(Configuration["TokenizationConfig:DatabaseConnection"]);

            services.RegisterCache(Configuration["TokenizationConfig:CacheType"], Configuration["TokenizationConfig:CacheConnection"]);

            services.RegisterRepositories();

            services.RegisterHSMChannel();

            services.RegisterFamilyProviders();

            services.RegisterAPIs(Configuration["TokenizationConfig:ServiceId"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreCard.Tesla.Tokenization v1"); c.RoutePrefix = string.Empty; });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
