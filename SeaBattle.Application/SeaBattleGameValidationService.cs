using SeaBattle.Application.Abstract;
using SeaBattle.Application.Contracts;
using SeaBattle.Application.Contracts.Models;
using SeaBattle.Application.Extensions;
using SeaBattle.Common.Extensions;
using System.Threading.Tasks;

namespace SeaBattle.Application
{
    public class SeaBattleGameValidationService : ISeeBattleGameService
    {
        private readonly ISeeBattleGameService _service;
        private readonly IValidationService _validationService;

        public SeaBattleGameValidationService(ISeeBattleGameService service, IValidationService validationService)
        {
            _service = service.NotNull(nameof(service));
            _validationService = validationService.NotNull(nameof(validationService));
        }
        public Task AddShips(ShipsCreationModel creationModel)
        {
            _validationService.Validate(creationModel).ThrowIfHasErrors();

            return _service.AddShips(creationModel);
        }

        public Task CreateGame(GameCreationModel creationModel)
        {
            _validationService.Validate(creationModel).ThrowIfHasErrors();

            return _service.CreateGame(creationModel);

        }
        public Task<ShotResultModel> Shot(ShotModel shotModel)
        {
            _validationService.Validate(shotModel).ThrowIfHasErrors();

            return _service.Shot(shotModel);
        }

        public Task FinishGame()
        {
            return _service.FinishGame();
        }

        public Task<GameStatsModel> GetGameStats()
        {
            return _service.GetGameStats();
        }
    }
}
