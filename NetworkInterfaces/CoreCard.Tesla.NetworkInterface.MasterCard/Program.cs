using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CoreCard.Tesla.NetworkInterface.MasterCard
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>();
             })
            .UseSerilog();
        }
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
    }
}
