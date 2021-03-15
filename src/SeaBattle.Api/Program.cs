using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using Microsoft.Extensions.Configuration.Json;

namespace SeaBattle.Api
{
    public class Program
    {
        static void Main(string[] args)
        {
            IHostBuilder hostBuilder = Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureLogging(ConfigureLogging)
                .ConfigureWebHostDefaults(ConfigureWebHost);

            using (IHost host = hostBuilder.Build())
            {
                host.Run();
            }
        }

        private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder config)
        {
            IConfigurationSource[] jsonFilesSources = config.Sources.Where(source => source is JsonConfigurationSource).ToArray();
            foreach (IConfigurationSource source in jsonFilesSources)
            {
                // remove all providers for json files so that later they can be rewritten below
                config.Sources.Remove(source);
            }
            
            string path = context.HostingEnvironment.ContentRootPath;
            string environment = context.HostingEnvironment.EnvironmentName;

            config
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false);
        }

        private static void ConfigureLogging(ILoggingBuilder builder)
        {
            builder.ClearProviders();
            builder.AddSimpleConsole(configure =>
            {
                configure.SingleLine = true;
                configure.UseUtcTimestamp = true;
                configure.ColorBehavior = LoggerColorBehavior.Enabled;
            });
        }

        private static void ConfigureWebHost(IWebHostBuilder builder)
        {
            // TODO Transfer to configuration
            const string HostName = "+";
            const int DefaultPort = 20270;


            UriBuilder uriBuilder = new UriBuilder()
            {
                Host = HostName,
                Port = DefaultPort,
                Scheme = Uri.UriSchemeHttp
            };

            builder.UseUrls(uriBuilder.ToString());
            builder.ConfigureKestrel(options => options.AddServerHeader = false);
            builder.UseStartup<Startup>();
        }
    }
}
