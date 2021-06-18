using System;
using CoreCard.Tesla.NetworkInterface.Communication;
using CoreCard.Tesla.NetworkInterface.IsoDataModels;
using CoreCard.Tesla.NetworkInterface.MasterCard.DataModels;
using CoreCard.Tesla.NetworkInterface.MasterCard.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

namespace CoreCard.Tesla.NetworkInterface.MasterCard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreCard.Tesla.NetworkInterface.MasterCard v1");
                        c.RoutePrefix = string.Empty;
                    }
                );

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoreCard.Tesla.NetworkInterface.MasterCard", Version = "v1" });
            });

            services.AddSingleton<INetworkChannel, NetworkChannel>();
            services.AddSingleton<StanGenerator>();
            services.AddSingleton<IIsoFieldFactory, IsoFieldFactory>();
            services.AddSingleton<IIsoMessageFactory, IsoMessageFactory>();
            services.AddSingleton<IIsoMessageStreamFactory, IsoMessageStreamFactory>();
            services.AddSingleton<ISocketClientFactory, SocketClientFactory>();
            services.AddSingleton<MasterCardNetworkSettings>();

            services.AddTransient<NetworkManagementRequest>();
            services.AddTransient<NetworkManagementResponse>();
            services.AddTransient<AuthorizationRequest>();
            services.AddTransient<AuthorizationResponse>();
            services.AddTransient<FinancialRequest>();
            services.AddTransient<FinancialResponse>();

            services.Configure<MasterCardNetworkSettings>(Configuration.GetSection(nameof(MasterCardNetworkSettings)));

            services.Configure<NetworkConnectionSettings>(Configuration.GetSection($"{nameof(MasterCardNetworkSettings)}:{nameof(NetworkConnectionSettings)}"));

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .CreateLogger();

            services.AddHostedService<NetworkChannelWorkerService>();
        }
    }
}
