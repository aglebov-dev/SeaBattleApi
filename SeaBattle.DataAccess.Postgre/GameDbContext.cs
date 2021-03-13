using Microsoft.EntityFrameworkCore;
using SeaBattle.DataAccess.Postgre.Entities;

namespace SeaBattle.DataAccess.Postgre
{
    public class GameDbContext : DbContext
    {
        public DbSet<GameEntity> Games { get; set; }
        public DbSet<ShipEntity> Ships { get; set; }
        public DbSet<ShotEntity> Shots { get; set; }

        public GameDbContext(DbContextOptions options)
            : base(options) { }
    }
}
