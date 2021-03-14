using SeaBattle.Api.Abstract;
using SeaBattle.Api.Contracts;
using SeaBattle.Application.Contracts.Models;
using SeaBattle.Common.Extensions;

namespace SeaBattle.Api.Services
{
    public class ApplicationMapper : IApplicationMapper
    {
        public GameCreationModel Map(MatrixCreationRequest request)
        {
            AssertExtensions.NotNull(request, nameof(request));

            return new GameCreationModel(request.Range);
        }

        public ShipsCreationModel Map(ShipRequest request)
        {
            AssertExtensions.NotNull(request, nameof(request));

            return new ShipsCreationModel(request.Coordinates);
        }

        public ShotModel Map(ShotRequest request)
        {
            AssertExtensions.NotNull(request, nameof(request));

            return new ShotModel(request.Coord);
        }
    }
}
