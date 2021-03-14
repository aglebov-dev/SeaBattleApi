using SeaBattle.Application.Abstract;
using SeaBattle.Application.Contracts.Models;
using SeaBattle.Common.Extensions;
using SeaBattle.Domain;

namespace SeaBattle.Application.InternalServices
{
    public class ModelsMapper : IModelsMapper
    {
        public ShotResultModel CreateShotResult(GameStatsDomainModel stats)
        {
            AssertExtensions.NotNull(stats, nameof(stats));

            return new ShotResultModel(false, false, stats.IsEnded);
        }

        public ShotResultModel CreateShotResult(ShipDomainModel ship, GameStatsDomainModel stats)
        {
            AssertExtensions.NotNull(ship, nameof(ship));
            AssertExtensions.NotNull(stats, nameof(stats));

            return new ShotResultModel(ship.Health == 0, true, stats.IsEnded);
        }

        public GameStatsModel Map(GameStatsDomainModel stats)
        {
            return new GameStatsModel(stats.ShipCount, stats.DestroyedCount, stats.KnockedCount, stats.ShotCount);
        }
    }
}
