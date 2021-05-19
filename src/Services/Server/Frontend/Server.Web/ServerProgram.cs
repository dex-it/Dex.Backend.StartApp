using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Server.Web
{
    public class ServerProgram
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddConfiguration(context.Configuration.GetSection("Logging"));
                    builder.ClearProviders();
                    builder.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<ServerStartup>();
                    webBuilder.ConfigureAppConfiguration((hostingContext, builder) =>
                    {
                        var hostingEnvironment = hostingContext.HostingEnvironment;
                        builder
                            .AddJsonFile("Server.appsettings.json", true, true)
                            .AddJsonFile($"Server.appsettings.{hostingEnvironment.EnvironmentName}.json", true)
                            .AddJsonFile("Server.appsettings.local.json", true)
                            .AddEnvironmentVariables();
                         
                        if (args == null)
                            return;
                    
                        builder.AddCommandLine(args);

                    });
                });
    }
}