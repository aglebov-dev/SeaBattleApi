using Microsoft.Extensions.DependencyInjection;
using SeaBattle.DataAccess.Contracts;

namespace SeaBattle.DataAccess.Memory
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Provides memory data access registration dependencies.
        /// </summary>
        public static IServiceCollection AddMemoryDataAccess(this IServiceCollection services)
        {
            services
                .AddSingleton<IGameRepository, MemoryGameRepository>();

            return services;
        }
    }
}
