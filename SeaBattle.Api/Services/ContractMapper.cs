using SeaBattle.Api.Abstract;
using SeaBattle.Api.Contracts;
using SeaBattle.Application.Contracts.Models;
using SeaBattle.Common.Extensions;

namespace SeaBattle.Api.Services
{
    public class ContractMapper : IContractMapper
    {
        public StatsResponse Map(GameStatsModel stats)
        {
            AssertExtensions.NotNull(stats, nameof(stats));

            return new StatsResponse
            {
                Destroyed = stats.Destroyed,
                Knocked = stats.Knocked,
                ShipsCount = stats.ShipCount,
                ShotsCount = stats.ShotCount
            };
        }

        public ShotResponse Map(ShotResultModel result)
        {
            AssertExtensions.NotNull(result, nameof(result));

            return new ShotResponse
            {
                Destroy = result.ShipDestroyed,
                Knock = result.Knock,
                End = result.GameEnded
            };
        }
    }
}
