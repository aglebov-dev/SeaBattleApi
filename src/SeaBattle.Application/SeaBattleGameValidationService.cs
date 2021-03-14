using SeaBattle.Application.Contracts;
using SeaBattle.Application.Contracts.Models;
using SeaBattle.Common.DataValidation;
using SeaBattle.Common.Extensions;
using SeaBattle.Domain.Exceptions;
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
            ThrowIfHasErrors(_validationService.Validate(creationModel));

            return _service.AddShips(creationModel);
        }

        public Task CreateGame(GameCreationModel creationModel)
        {
            ThrowIfHasErrors(_validationService.Validate(creationModel));

            return _service.CreateGame(creationModel);

        }
        public Task<ShotResultModel> Shot(ShotModel shotModel)
        {
            ThrowIfHasErrors(_validationService.Validate(shotModel));

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

        private void ThrowIfHasErrors(ValidationData result)
        {
            if (!result.Success)
            {
                throw new DataValidationException(result.Errors);
            }
        }
    }
}
