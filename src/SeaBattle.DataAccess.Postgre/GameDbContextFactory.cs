using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace SeaBattle.DataAccess.Postgre
{
    public class GameDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
    {
        public GameDbContext CreateDbContext(string[] args)
        {
            string connectionString = "Server=127.0.0.1;Database=game;password=root;user id=root;Port=5432;Integrated Security=True";
            DbContextOptionsBuilder<GameDbContext> optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            optionsBuilder
                .ConfigureWarnings(b => b.Default(WarningBehavior.Throw))
                .UseNpgsql(connectionString, OptionsConfiguration);

            return new GameDbContext(optionsBuilder.Options);
        }

        private static void OptionsConfiguration(NpgsqlDbContextOptionsBuilder builder)
        {
            string assemblyName = typeof(GameDbContext).Namespace;
            builder.MigrationsHistoryTable("__ef_migrations_history", StorageConstants.Schema);
            builder.MigrationsAssembly(assemblyName);
        }
    }
}
