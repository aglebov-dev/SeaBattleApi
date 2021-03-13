using SeaBattle.Application.Abstract;
using SeaBattle.Application.Contracts;
using SeaBattle.Application.Contracts.Models;
using SeaBattle.Common.Extensions;
using SeaBattle.DataAccess.Contracts;
using SeaBattle.Domain;
using SeaBattle.Domain.Abstract;
using SeaBattle.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeaBattle.Application
{
    public class SeaBattleGameService : ISeeBattleGameService
    {
        private readonly IGameRepository _gameStateRepository;
        private readonly ICoordinatesParser _coordinatesParser;
        private readonly IModelsMapper _modelMapper;

        public SeaBattleGameService(
            IGameRepository gameStateRepository,
            ICoordinatesParser coordinatesParser,
            IModelsMapper modelMapper)
        {
            _gameStateRepository = gameStateRepository.NotNull(nameof(gameStateRepository));
            _coordinatesParser = coordinatesParser.NotNull(nameof(coordinatesParser));
            _modelMapper = modelMapper.NotNull(nameof(modelMapper));
        }

        public async Task CreateGame(GameCreationModel creationModel)
        {
            AssertExtensions.NotNull(creationModel, nameof(creationModel));

            await ThrowIfActiveGameAlreadyExist();

            await _gameStateRepository.AddGame(creationModel.Size);
        }

        // TODO UnitOfWork
        public async Task AddShips(ShipsCreationModel creationModel)
        {
            IReadOnlyCollection<ShipDomainModel> ships = _coordinatesParser.ParseShipsCoordinates(creationModel.Coordinates);
            GameDomainModel game = await GetActiveGame();

            ThrowIfCanNotAddShips(game, ships);

            await _gameStateRepository.AddShips(game, ships);
        }

        // TODO UnitOfWork
        public async Task<ShotResultModel> Shot(ShotModel shotModel)
        {
            ShotDomainModel shot = _coordinatesParser.ParseCoordinate(shotModel.Coord);
            GameDomainModel game = await GetActiveGame();

            await ThrowIfShotCanNotAdded(shot, game);

            ShipDomainModel ship = await GetKnockedShip(game, shot);

            await _gameStateRepository.AddShot(game, shot, ship);
            
            GameStatsDomainModel stats = await _gameStateRepository.GetGameStats(game);
            if (stats.IsEnded)
            {
                await _gameStateRepository.EndGame(game);
            }

            return _modelMapper.CreateShotResult(ship?.With(shot), stats);
        }

        public async Task FinishGame()
        {
            GameDomainModel game = await GetActiveGame();
            await _gameStateRepository.FinishGame(game);
        }

        public async Task<GameStatsModel> GetGameStats()
        {
            GameDomainModel game = await GetActiveGame();
            GameStatsDomainModel stats = await _gameStateRepository.GetGameStats(game);
            GameStatsModel result = _modelMapper.Map(stats);

            return result;
        }

        private Task<ShipDomainModel> GetKnockedShip(GameDomainModel game, ShotDomainModel shot)
        {
            SearchShipCriteria criteria = new SearchShipCriteria(game, shot.Point);

            return _gameStateRepository.TryGetShip(criteria);
        }

        private async Task ThrowIfShotCanNotAdded(ShotDomainModel shot, GameDomainModel game)
        {
            if (!game.Init)
            {
                throw new DataValidationException("The game is not ready to play.");
            }
            if (game.Ended)
            {
                throw new DataValidationException("The game is ended.");
            }
            if (!game.CanTakeShot(shot))
            {
                throw new DataValidationException("The shot does not match the parameters of the game.");
            }

            ShotDomainModel existShot = await _gameStateRepository.TryGetShot(game, shot.Point.X, shot.Point.Y);
            if (existShot != null)
            {
                throw new DataValidationException("You can not repeat shots.");
            }
        }

        private async Task ThrowIfActiveGameAlreadyExist()
        {
            IReadOnlyCollection<GameDomainModel> games = await _gameStateRepository.GetActiveGames();
            if (games?.Count > 0)
            {
                throw new InvalidGameOperationException("There is already an active game.");
            }
        }

        private void ThrowIfCanNotAddShips(GameDomainModel game, IEnumerable<ShipDomainModel> ships)
        {
            if (game.Init)
            {
                throw new DataValidationException("Sheeps was already added.");
            }
            if (game.Ended)
            {
                throw new DataValidationException("The game is over.");
            }
            if (!game.CanAddShips(ships))
            {
                // TODO if needs to get specific errors, then you need to add a service to the domain 
                //      that will calculate the result of adding ships
                throw new DataValidationException("The specified ships cannot be added to the game.");
            }
        }

        private async Task<GameDomainModel> GetActiveGame()
        {
            IReadOnlyCollection<GameDomainModel> ids = await _gameStateRepository.GetActiveGames();
            if (ids is null || ids.Count < 1)
            {
                throw new NotFoundException("There is no active game.");
            }

            return ids.Single();
        }
    }
}
