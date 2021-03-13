using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeaBattle.Api.Configurations;
using System;
using System.IO;
using System.Reflection;

namespace SeaBattle.Api.Extensions
{
    public static class SwaggerServiceCollectionExtensions
    {
        /// <summary>
        /// Provides swagger configuration.
        /// </summary>
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            ExtendedOpenApiInfo openApiInfo = configuration.GetSection("Swagger").Get<ExtendedOpenApiInfo>();

            return services
                .AddSwaggerGenNewtonsoftSupport()
                .AddSwaggerGen(options =>
                {
                    if (!string.IsNullOrWhiteSpace(openApiInfo?.UriResource))
                    {
                        options.SwaggerDoc(openApiInfo.UriResource, openApiInfo);
                    }

                    //TODO Think, maybe needs to download all the documentation that is in the application folder
                    string xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                    if (File.Exists(xmlPath))
                    {
                        options.IncludeXmlComments(xmlPath);
                    }
                });
        }
    }
}
