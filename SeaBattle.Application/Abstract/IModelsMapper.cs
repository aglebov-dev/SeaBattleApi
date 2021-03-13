using SeaBattle.Application.Contracts.Models;
using SeaBattle.Domain;

namespace SeaBattle.Application.Abstract
{
    public interface IModelsMapper
    {
        GameStatsModel Map(GameStatsDomainModel stats);
        ShotResultModel CreateShotResult(ShipDomainModel ship, GameStatsDomainModel stats);
    }
}
