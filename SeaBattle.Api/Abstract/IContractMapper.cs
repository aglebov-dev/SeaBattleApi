using SeaBattle.Api.Contracts;
using SeaBattle.Application.Contracts.Models;

namespace SeaBattle.Api.Abstract
{
    public interface IContractMapper
    {
        StatsResponse Map(GameStatsModel stats);
        ShotResponse Map(ShotResultModel result);
    }
}
