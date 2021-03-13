using SeaBattle.Application.Abstract;
using SeaBattle.Application.Contracts.Models;
using SeaBattle.Common.Extensions;
using SeaBattle.Domain;

namespace SeaBattle.Application.InternalServices
{
    public class ModelsMapper : IModelsMapper
    {
        public ShotResultModel CreateShotResult(ShipDomainModel ship, GameStatsDomainModel stats)
        {
            AssertExtensions.NotNull(stats, nameof(stats));

            var destroed = ship != null && ship.Health == 0;
            var knocked = ship != null;

            return new ShotResultModel(destroed, knocked, stats.IsEnded);
        }

        public GameStatsModel Map(GameStatsDomainModel stats)
        {
            return new GameStatsModel(stats.ShipCount, stats.DestroyedCount, stats.KnockedCount, stats.ShotCount);
        }
    }
}
