using System.Collections.Generic;

namespace SeaBattle.Domain.Abstract
{
    public interface ICoordinatesParser
    {
        IReadOnlyCollection<ShipDomainModel> ParseShipsCoordinates(string text);
        ShotDomainModel ParseCoordinate(string text);
    }
}
