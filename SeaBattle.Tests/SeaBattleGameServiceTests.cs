using FakeItEasy;
using SeaBattle.Application;
using SeaBattle.Application.Abstract;
using SeaBattle.Application.Contracts.Models;
using SeaBattle.Common.Types;
using SeaBattle.DataAccess.Contracts;
using SeaBattle.Domain;
using SeaBattle.Domain.Abstract;
using SeaBattle.Domain.Exceptions;
using System.Threading.Tasks;
using Xunit;

namespace SeaBattle.Tests
{
    public class SeaBattleGameServiceTests
    {
        private class TestClass : SeaBattleGameService
        {
            public IGameRepository GameStateRepository { get; }
            public ICoordinatesParser CoordinatesParser { get; }
            public IModelsMapper ModelMapper { get; }

            public TestClass(IGameRepository gameStateRepository, ICoordinatesParser coordinatesParser, IModelsMapper modelMapper)
                : base(gameStateRepository, coordinatesParser, modelMapper)
            {
                GameStateRepository = gameStateRepository;
                CoordinatesParser = coordinatesParser;
                ModelMapper = modelMapper;
            }
        }

        private TestClass Create()
        {
            IGameRepository gameStateRepository = A.Fake<IGameRepository>();
            ICoordinatesParser coordinatesParser = A.Fake<ICoordinatesParser>();
            IModelsMapper modelMapper = A.Fake<IModelsMapper>();

            return new TestClass(gameStateRepository, coordinatesParser, modelMapper);
        }

        [Fact]
        public async Task CreateGame_WhenActiveGameExist_ThrowException()
        {
            TestClass service = Create();
            A.CallTo(() => service.GameStateRepository.GetActiveGames())
                .Returns(new[] { new GameDomainModel(1, 1, false, false) });

            await Assert.ThrowsAsync<InvalidGameOperationException>(() => service.CreateGame(new GameCreationModel(0)));
        }

        [Fact]
        public async Task CreateGame_WhenActiveGameNotExist_InvokeRepo()
        {
            TestClass service = Create();
            var call = A.CallTo(() => service.GameStateRepository.AddGame(0));

            await service.CreateGame(new GameCreationModel(0));

            call.MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddShips_WhenActiveGameNotExist_ThrowException()
        {
            TestClass service = Create();
            A.CallTo(() => service.GameStateRepository.GetActiveGames())
                .Returns(new GameDomainModel[] { });

            await Assert.ThrowsAsync<NotFoundException>(() => service.AddShips(new ShipsCreationModel("")));
        }

        [Fact]
        public async Task AddShips_WhenGameInit_ThrowException()
        {
            TestClass service = Create();
            var activeGame = new GameDomainModel(id: 1, size: 1, init: true, ended: false);

            A.CallTo(() => service.GameStateRepository.GetActiveGames())
                .Returns(new[] { activeGame });

            await Assert.ThrowsAsync<DataValidationException>(() => service.AddShips(new ShipsCreationModel("")));
        }

        [Fact]
        public async Task AddShips_WhenGameEnded_ThrowException()
        {
            TestClass service = Create();
            var activeGame = new GameDomainModel(id: 1, size: 1, init: false, ended: true);

            A.CallTo(() => service.GameStateRepository.GetActiveGames())
                .Returns(new[] { activeGame });

            await Assert.ThrowsAsync<DataValidationException>(() => service.AddShips(new ShipsCreationModel("")));
        }

        [Fact]
        public async Task AddShips_WhenGameSuitable_InvokeRepo()
        {
            TestClass service = Create();
            var activeGame = new GameDomainModel(id: 1, size: 1, init: false, ended: false);
            var call = A.CallTo(() => service.GameStateRepository.AddShips(default, default)).WithAnyArguments();
            A.CallTo(() => service.GameStateRepository.GetActiveGames())
                .Returns(new[] { activeGame });

            await service.AddShips(new ShipsCreationModel(""));

            call.MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Shot_WhenDuplicateShot_ThrowException()
        {
            TestClass service = Create();
            var activeGame = new GameDomainModel(id: 1, size: 1, init: true, ended: false);
            A.CallTo(() => service.GameStateRepository.GetActiveGames())
                .Returns(new[] { activeGame });
            A.CallTo(() => service.GameStateRepository.TryGetShot(default ,0, 0))
                .WithAnyArguments()
                .Returns(new ShotDomainModel(new Point(5, 9)));

            await Assert.ThrowsAsync<DataValidationException>(() => service.Shot(new ShotModel("1A")));
        }

        [Fact]
        public async Task Shot_WhenCorrectData_InvokeRepo()
        {
            TestClass service = Create();
            var activeGame = new GameDomainModel(id: 1, size: 1, init: true, ended: false);
            A.CallTo(() => service.GameStateRepository.GetActiveGames())
                .Returns(new[] { activeGame });
            A.CallTo(() => service.GameStateRepository.TryGetShot(default, 0, 0))
                .WithAnyArguments()
                .Returns(default(ShotDomainModel));

            var call1 = A.CallTo(() => service.GameStateRepository.AddShot(default, default, default)).WithAnyArguments();
            var call2 = A.CallTo(() => service.GameStateRepository.TryGetShip(default)).WithAnyArguments();
            var call3 = A.CallTo(() => service.GameStateRepository.GetGameStats(default)).WithAnyArguments();

            await service.Shot(new ShotModel(""));

            call1.MustHaveHappenedOnceExactly();
            call2.MustHaveHappenedOnceExactly();
            call3.MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Shot_WhenLastShot_InvokeRepoEndGame()
        {
            TestClass service = Create();
            var activeGame = new GameDomainModel(id: 1, size: 1, init: true, ended: false);
            A.CallTo(() => service.GameStateRepository.GetActiveGames())
                .Returns(new[] { activeGame });
            A.CallTo(() => service.GameStateRepository.TryGetShot(default, 0, 0))
                .WithAnyArguments()
                .Returns(default(ShotDomainModel));
            A.CallTo(() => service.GameStateRepository.GetGameStats(default))
                .WithAnyArguments()
                .Returns(new GameStatsDomainModel(0, 0, 0, 0, true));

            var call = A.CallTo(() => service.GameStateRepository.EndGame(default)).WithAnyArguments();

            await service.Shot(new ShotModel(""));

            call.MustHaveHappenedOnceExactly();
        }
    }
}
