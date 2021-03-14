using SeaBattle.Application.Contracts.Models;
using System.Threading.Tasks;

namespace SeaBattle.Application.Contracts
{
    public interface ISeeBattleGameService
    {
        Task CreateGame(GameCreationModel creationModel);
        Task AddShips(ShipsCreationModel creationModel);
        Task<ShotResultModel> Shot(ShotModel shotModel);
        Task FinishGame();
        Task<GameStatsModel> GetGameStats();
    }
}
