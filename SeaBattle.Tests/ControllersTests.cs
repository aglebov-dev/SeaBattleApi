using FakeItEasy;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SeaBattle.Api;
using SeaBattle.Api.Controllers;
using SeaBattle.Application.Contracts.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SeaBattle.Tests
{
    public class ControllersTestHelper
    {
        private class TestWebHostEnvironment : IWebHostEnvironment
        {
            public string WebRootPath { get; set; }
            public string EnvironmentName { get; set; }
            public string ApplicationName { get; set; }
            public string ContentRootPath { get; set; }
            public IFileProvider WebRootFileProvider { get; set; }
            public IFileProvider ContentRootFileProvider { get; set; }
        }

        private readonly IServiceCollection _serviceCollection;
        private readonly ServiceProvider _serviceProvider;

        public ControllersTestHelper()
        {
            var testConfiguration = new Dictionary<string, string>
            {
                ["ConnectionStrings:Postgre"] = "Server=127.0.0.1;Database=game;Port=5432;Integrated Security=True",
                [$"{nameof(StorageConfiguration)}:{nameof(StorageConfiguration.UseMemoryStorage)}"] = "true"
            };

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(testConfiguration)
                .Build();
            IWebHostEnvironment webHostEnvironment = new TestWebHostEnvironment();
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddSingleton(webHostEnvironment)
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<Startup>();

            IEnumerable<Type> controllers = typeof(Startup).Assembly
                .GetTypes()
                .Where(x => x.IsSameOrInherits(typeof(ControllerBase)));

            foreach (Type controllerType in controllers)
            {
                serviceCollection.AddScoped(controllerType);
            }

            Startup startUp = serviceCollection.BuildServiceProvider().GetRequiredService<Startup>();
            startUp.ConfigureServices(serviceCollection);

            _serviceCollection = serviceCollection;
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Theory]
        [InlineData(typeof(GameController))]
        public void ControllerInitializeTests(Type controllerType)
        {
            object controller = _serviceProvider.GetRequiredService(controllerType);

            Assert.NotNull(controller);
        }

        [Fact]
        public void ServiceCollectionTypesTest()
        {
            IEnumerable<ServiceDescriptor> typeDescriptors = _serviceCollection
               .Where(descriptor => descriptor.ServiceType.Namespace.StartsWith("SeaBattle"));

            foreach (ServiceDescriptor item in typeDescriptors)
            {
                object service = _serviceProvider.GetRequiredService(item.ServiceType);

                Assert.NotNull(service);
            }
        }
    }
}
