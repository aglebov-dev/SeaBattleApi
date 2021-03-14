using SeaBattle.Api.Contracts;
using SeaBattle.Application.Contracts.Models;

namespace SeaBattle.Api.Abstract
{
    public interface IApplicationMapper
    {
        GameCreationModel Map(MatrixCreationRequest request);
        ShipsCreationModel Map(ShipRequest request);
        ShotModel Map(ShotRequest request);
    }
}
