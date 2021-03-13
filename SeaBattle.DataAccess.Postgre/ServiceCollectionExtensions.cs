using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using SeaBattle.DataAccess.Contracts;

namespace SeaBattle.DataAccess.Postgre
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Provides postgre data access registration dependencies.
        /// </summary>
        public static IServiceCollection AddPostgreDataAccess(this IServiceCollection services)
        {
            services.AddDbContextPool<GameDbContext>((provider, options) =>
            {
                string connectionString = provider
                    .GetRequiredService<IConfiguration>()
                    .GetConnectionString(StorageConstants.ConnectionStringName);

                options
                    .ConfigureWarnings(b => b.Default(WarningBehavior.Throw))
                    .UseNpgsql(connectionString, OptionsConfiguration);
            });

            services
                .AddScoped<IGameRepository, GameRepository>();

            return services;
        }

        private static void OptionsConfiguration(NpgsqlDbContextOptionsBuilder builder)
        {
            string assemblyName = typeof(GameDbContext).Namespace;

            builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            builder.MigrationsHistoryTable("__ef_migrations_history", StorageConstants.Schema);
            builder.MigrationsAssembly(assemblyName);
        }
    }
}
