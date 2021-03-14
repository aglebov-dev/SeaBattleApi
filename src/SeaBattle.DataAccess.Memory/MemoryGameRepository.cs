using SeaBattle.Common.Types;
using SeaBattle.DataAccess.Contracts;
using SeaBattle.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SeaBattle.DataAccess.Memory
{
    public class MemoryGameRepository : IGameRepository
    {
        private long gameId = 0;
        private long shipId = 0;
        private readonly ConcurrentDictionary<long, GameDomainModel> _games;
        private readonly ConcurrentDictionary<long, List<ShotDomainModel>> _shots;
        private readonly ConcurrentDictionary<long, List<ShipDomainModel>> _ships;

        public MemoryGameRepository()
        {
            _games = new ConcurrentDictionary<long, GameDomainModel>();
            _shots = new ConcurrentDictionary<long, List<ShotDomainModel>>();
            _ships = new ConcurrentDictionary<long, List<ShipDomainModel>>();
        }

        public Task AddGame(int size)
        {
            long id = Interlocked.Increment(ref gameId);
            GameDomainModel game = new GameDomainModel(id, size, false, false);
            _games.AddOrUpdate(id, game, (id, g) => game);

            return Task.CompletedTask;
        }

        public Task AddShips(GameDomainModel game, IReadOnlyCollection<ShipDomainModel> ships)
        {
            ThrowIfGameNotExists(game);

            IEnumerable<ShipDomainModel> localShips = ships.Select(ship => ship.With(Interlocked.Increment(ref shipId)));
            _ships.AddOrUpdate(game.Id, localShips.ToList(), (id, list) =>
            {
                list.AddRange(localShips);
                return list;
            });

            var newGame = new GameDomainModel(game.Id, game.Size, true, game.Ended);
            _games.AddOrUpdate(game.Id, id => newGame, (id, g) => newGame);

            return Task.CompletedTask;
        }


        public Task AddShot(GameDomainModel game, ShotDomainModel shot, ShipDomainModel ship = null)
        {
            ThrowIfGameNotExists(game);

            var localShot = new ShotDomainModel(new Point(shot.Point.X, shot.Point.Y));
            _shots.AddOrUpdate(game.Id, new List<ShotDomainModel> { localShot }, (id, list) =>
            {
                list.Add(localShot);
                return list;
            });

            if (ship != null)
            {
                _ = _ships.AddOrUpdate(game.Id, id => throw GetDefaultException(), (id, list) =>
                {
                    ShipDomainModel oldShip = list.Where(s => s.Id == ship.Id).Single();
                    ShipDomainModel newShip = oldShip.With(shot);
                    list.Remove(oldShip);
                    list.Add(newShip);

                    return list;
                });
            }

            return Task.CompletedTask;
        }

        public Task EndGame(GameDomainModel game)
        {
            ThrowIfGameNotExists(game);

            _games.AddOrUpdate(
                game.Id,
                id => throw GetDefaultException(),
                (id, g) => new GameDomainModel(g.Id, g.Size, g.Init, true)
            );

            return Task.CompletedTask;
        }

        public Task FinishGame(GameDomainModel game)
        {
            ThrowIfGameNotExists(game);

            _ = _games.Remove(game.Id, out GameDomainModel _);
            _ = _shots.Remove(game.Id, out List<ShotDomainModel> _);
            _ = _ships.Remove(game.Id, out List<ShipDomainModel> _);

            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<GameDomainModel>> GetActiveGames()
        {
            IReadOnlyCollection<GameDomainModel> games = _games.Values.ToImmutableList();

            return Task.FromResult(games);
        }

        public Task<GameStatsDomainModel> GetGameStats(GameDomainModel game)
        {
            ThrowIfGameNotExists(game);

            GameDomainModel localGame = _games.GetOrAdd(game.Id, id => throw GetDefaultException());
            _shots.TryGetValue(game.Id, out var localShots);
            _ships.TryGetValue(game.Id, out var localShips);

            ShipDomainModel[] destroed = localShips is null
                ? Array.Empty<ShipDomainModel>()
                : localShips.Where(s => s.Health == 0).ToArray();

            ShipDomainModel[] knoked = localShips is null
                ? Array.Empty<ShipDomainModel>()
                : localShips.Except(destroed).Where(s => s.Health != s.MaxHealth).ToArray();

            var shipCount = localShips?.Count ?? 0;
            var shotCount = localShots?.Count ?? 0;

            bool ended = shipCount == destroed.Length;
            var stats = new GameStatsDomainModel(shipCount, destroed.Length, knoked.Length, shotCount, ended);

            return Task.FromResult(stats);
        }

        public Task<ShotDomainModel> TryGetShot(GameDomainModel game, int x, int y)
        {
            ThrowIfGameNotExists(game);

            _shots.TryGetValue(game.Id, out var localShots);

            var shot = localShots?
                .Where(s => s.Point.X == x)
                .Where(s => s.Point.Y == y)
                .FirstOrDefault();

            return Task.FromResult(shot);
        }

        public Task<ShipDomainModel> TryGetShip(SearchShipCriteria criteria)
        {
            ThrowIfGameNotExists(criteria.Game);

            long gameId = criteria.Game.Id;
            int x = criteria.Coordinate.X;
            int y = criteria.Coordinate.Y;

            var ships = _ships.GetOrAdd(gameId, id => throw GetDefaultException());

            var ship = ships
                .Where(s => x >= s.Start.X && y >= s.Start.Y)
                .Where(s => x <= s.End.X && y <= s.End.Y)
                .FirstOrDefault();

            return Task.FromResult(ship);
        }

        private void ThrowIfGameNotExists(GameDomainModel game)
        {
            if (!_games.ContainsKey(game.Id))
            {
                throw GetDefaultException();
            }
        }

        private Exception GetDefaultException()
        {
            return new ApplicationException("Unknown game.");
        }
    }
}
