using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SeaBattle.DataAccess.Postgre
{
    public class GameDbContextFactory : IDesignTimeDbContextFactory<GameDbContext>
    {
        public GameDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<GameDbContext> optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            optionsBuilder
                .ConfigureWarnings(b => b.Default(WarningBehavior.Throw))
                .UseNpgsql("Server=127.0.0.1;Database=game;password=root;user id=root;Port=5432;Integrated Security=True");

            return new GameDbContext(optionsBuilder.Options);
        }
    }
}
