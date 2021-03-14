using Microsoft.EntityFrameworkCore;
using SeaBattle.Common.Types;
using SeaBattle.DataAccess.Contracts;
using SeaBattle.DataAccess.Postgre.Entities;
using SeaBattle.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeaBattle.DataAccess.Postgre
{
    public class GameRepository : IGameRepository
    {
        private readonly GameDbContext _context;

        public GameRepository(GameDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<GameDomainModel>> GetActiveGames()
        {
            return await _context.Games
                .Where(game => !game.Finished)
                .OrderBy(game => game.Id)
                .AsNoTracking()
                .Select(game => new GameDomainModel(game.Id, game.Size, game.Init, game.Ended))
                .ToListAsync();
        }

        public Task AddGame(int size)
        {
            GameEntity game = new GameEntity
            {
                Size = size,
                Init = false,
                Ended = false
            };

            _context.Games.Add(game);

            return _context.SaveChangesAsync();
        }

        public Task AddShips(GameDomainModel game, IReadOnlyCollection<ShipDomainModel> ships)
        {
            GameEntity gameEntity = new GameEntity
            {
                Id = game.Id,
                Init = true
            };

            IEnumerable<ShipEntity> entities = ships.Select(ship => new ShipEntity
            {
                GameId = game.Id,
                XStart = ship.Start.X,
                YStart = ship.Start.Y,
                XEnd = ship.End.X,
                YEnd = ship.End.Y
            });

            _context.Entry(gameEntity).Property(game => game.Init).IsModified = true;
            _context.Ships.AddRange(entities);

            return _context.SaveChangesAsync();
        }

        public Task AddShot(GameDomainModel game, ShotDomainModel shot, ShipDomainModel ship = null)
        {
            ShotEntity shotEntity = new ShotEntity
            {
                ShipId = ship?.Id,
                GameId = game.Id,
                X = shot.Point.X,
                Y = shot.Point.Y
            };

            _context.Shots.Add(shotEntity);

            return _context.SaveChangesAsync();
        }

        public async Task<ShipDomainModel> TryGetShip(SearchShipCriteria criteria)
        {
            long gameId = criteria.Game.Id;
            int x = criteria.Coordinate.X;
            int y = criteria.Coordinate.Y;

            ShipEntity shipEntity = await _context.Ships
                .Where(ship => ship.GameId == gameId)
                .Where(ship => x >= ship.XStart && y >= ship.YStart)
                .Where(ship => x <= ship.XEnd && y <= ship.YEnd)
                .Include(ship => ship.Shots)
                .OrderBy(ship => ship.Id)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            if (shipEntity is null)
            {
                return null;
            }

            return CreateShip(shipEntity);
        }

        public Task EndGame(GameDomainModel board)
        {
            GameEntity entity = new GameEntity
            {
                Id = board.Id,
                Ended = true
            };

            _context.Entry(entity).Property(x => x.Ended).IsModified = true;

            return _context.SaveChangesAsync();
        }

        public Task FinishGame(GameDomainModel game)
        {
            GameEntity gameEntity = new GameEntity
            {
                Id = game.Id,
                Finished = true
            };

            _context.Entry(gameEntity).Property(x => x.Finished).IsModified = true;

            return _context.SaveChangesAsync();
        }

        public async Task<GameStatsDomainModel> GetGameStats(GameDomainModel game)
        {
            long gameId = game.Id;

            int shotsCount = await _context.Games
                .Where(game => game.Id == gameId)
                .SelectMany(game => game.Shots)
                .CountAsync();

            List<ShipEntity> shipsEntities = await _context.Ships
                .Where(ship => ship.GameId == gameId)
                .Include(ship => ship.Shots)
                .OrderBy(ship => ship.Id)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

            List<ShipDomainModel> ships = shipsEntities.Select(CreateShip).ToList();
            List<ShipDomainModel> destroed = ships.Where(s => s.Health == 0).ToList();
            List<ShipDomainModel> knoked = ships.Except(destroed).Where(s => s.Health != s.MaxHealth).ToList();
            bool ended = ships.Count == destroed.Count;

            return new GameStatsDomainModel(ships.Count, destroed.Count, knoked.Count, shotsCount, ended);
        }

        public async Task<ShotDomainModel> TryGetShot(GameDomainModel game, int x, int y)
        {
            ShotEntity entity = await _context.Shots
                .Where(shot => shot.GameId == game.Id)
                .Where(shot => shot.X == x)
                .Where(shot => shot.Y == y)
                .OrderBy(shot => shot.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (entity is null)
            {
                return null;
            }

            return new ShotDomainModel(new Point(entity.X, entity.Y));
        }

        private ShipDomainModel CreateShip(ShipEntity ship)
        {
            Point start = new Point(ship.XStart, ship.YStart);
            Point end = new Point(ship.XEnd, ship.YEnd);
            IEnumerable<ShotDomainModel> shotsModels = ship.Shots.Select(shot =>
            {
                Point point = new Point(shot.X, shot.Y);
                return new ShotDomainModel(point);
            });

            return new ShipDomainModel(ship.Id, start, end, shotsModels.ToArray());
        }
    }
}
