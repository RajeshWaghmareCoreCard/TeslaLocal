using CoreCard.Telsa.Cache;
using CoreCard.Tesla.Falcon.ADORepository;
using CoreCard.Tesla.Falcon.DbRepository.RepoInterfaces;
using CoreCard.Tesla.Falcon.NpgRepository;
using CoreCard.Tesla.Falcon.Services;
using CoreCard.Tesla.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
namespace CoreCard.Tesla.Falcon
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoreCard.Tesla.Falcon", Version = "v1" });
            });
            services.AddTransient<TimeLogger>();

            services.RegisterConfigs(Configuration);

            services.RegisterCache(Configuration["FalconAppSetting:CacheType"], Configuration["TokenizationConfig:CacheConnection"]);

            services.RegisterDatabase(Configuration["FalconAppSetting:DatabaseConnection"]);

            services.RegisterHttpClients(Configuration);

            services.RegisterRequestExtensions();

            services.RegisterTokenizationUtilChannel(Configuration);

            services.RegisterRepositories();

            services.RegisterAdcs();

            services.RegisterFalconServices();

            services.InitApplicationCache();

            //Added by Suraj Kovoor
            services.RegisterADORepositoryDI();
            services.RegisterBALDI();
            services.RegisterNpgDatabase(Configuration.GetConnectionString("CockroachDb"));
            services.RegisterNpgRepositoryDI();

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreCard.Tesla.Falcon v1"); c.RoutePrefix = string.Empty; });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.InitAppliationCache();

        }
    }
}
