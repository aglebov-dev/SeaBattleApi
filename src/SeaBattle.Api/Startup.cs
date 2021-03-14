using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeaBattle.Api.Abstract;
using SeaBattle.Api.Configurations;
using SeaBattle.Api.Extensions;
using SeaBattle.Api.Infrastructure;
using SeaBattle.Api.Infrastructure.ExceptionHandlers;
using SeaBattle.Api.Services;
using SeaBattle.Application.Dependencies;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SeaBattle.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual void Configure(IApplicationBuilder application)
        {
            application
                .UseMiddleware<ExceptionMiddleware>();

            application
                .UseRouting()
                .UseSwagger()
                .UseSwaggerUI(ConfigureSwaggerUI)
                .UseEndpoints(config => config.MapControllers());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions()
                .AddLogging()
                .AddSwagger(Configuration)
                .AddControllers()
                .AddControllersAsServices()
                .AddNewtonsoftJson(ConfigureJsonSerializer);

            services
                .AddSingleton<IApplicationMapper, ApplicationMapper>()
                .AddSingleton<IContractMapper, ContractMapper>();

            services
                .AddSingleton<IHttpExceptionHandler, ArgumentNullExceptionHandler>()
                .AddSingleton<IHttpExceptionHandler, DataValidationExceptionHandler>()
                .AddSingleton<IHttpExceptionHandler, SimpleExceptionHandler>()
                .AddSingleton<IHttpExceptionHandler, UnexpectedExceptionHandler>();

            services
                .AddSeaBattleApplication(Configuration);
        }

        private void ConfigureJsonSerializer(MvcNewtonsoftJsonOptions options)
        {
            options.SerializerSettings.ApplyDefaultJsonSettings();
        }

        private void ConfigureSwaggerUI(SwaggerUIOptions options)
        {
            ExtendedOpenApiInfo openApiInfo = Configuration.GetSection("Swagger").Get<ExtendedOpenApiInfo>();
            if (!string.IsNullOrWhiteSpace(openApiInfo?.UriResource))
            {
                string name = openApiInfo.Name ?? "Unknown";
                options.SwaggerEndpoint($"/swagger/{openApiInfo.UriResource}/swagger.json", name);
            }
        }
    }
}
