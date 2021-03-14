using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeaBattle.Application.Abstract;
using SeaBattle.Application.Contracts;
using SeaBattle.Application.Contracts.Configurations;
using SeaBattle.Application.InternalServices;
using SeaBattle.Application.Validators;
using SeaBattle.Common.DataValidation;
using SeaBattle.Common.Extensions;
using SeaBattle.DataAccess.Memory;
using SeaBattle.DataAccess.Postgre;
using SeaBattle.Domain.Abstract;

namespace SeaBattle.Application.Dependencies
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Provides registration dependencies for Sea Battle application.
        /// </summary>
        public static IServiceCollection AddSeaBattleApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSingleton<IValidationService, ValidationService>()
                .AddSingleton<IModelValidator, BoardCreationModelValidator>()
                .AddSingleton<IModelValidator, ShipsCreationModelValidator>()
                .AddSingleton<IModelValidator, ShotModelValidator>()
                .AddSingleton<IModelsMapper, ModelsMapper>();

            services
                .AddScoped<ISeeBattleGameService, SeaBattleGameService>()
                .AddSingleton<ICoordinatesParser, CoordinatesParser>();

            StorageConfiguration useMemoryStorage = configuration
                .GetSection(nameof(StorageConfiguration))
                .Get<StorageConfiguration>();

            if (useMemoryStorage?.UseMemoryStorage ?? false)
            {
                services.AddMemoryDataAccess();
            }
            else
            {
                services.AddPostgreDataAccess();
            }

            services
                .Decorate<ISeeBattleGameService, SeaBattleGameValidationService>();

            return services;
        }
    }
}
