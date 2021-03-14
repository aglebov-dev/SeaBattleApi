using SeaBattle.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeaBattle.DataAccess.Contracts
{
    public interface IGameRepository
    {
        Task<IReadOnlyCollection<GameDomainModel>> GetActiveGames();
        Task AddGame(int size);
        Task FinishGame(GameDomainModel game);
        Task AddShips(GameDomainModel game, IReadOnlyCollection<ShipDomainModel> ships);
        Task<GameStatsDomainModel> GetGameStats(GameDomainModel game);
        Task<ShipDomainModel> TryGetShip(SearchShipCriteria criteria);
        Task AddShot(GameDomainModel game, ShotDomainModel shot, ShipDomainModel ship = null);
        Task<ShotDomainModel> TryGetShot(GameDomainModel game, int x, int y);
        Task EndGame(GameDomainModel game);
    }
}
