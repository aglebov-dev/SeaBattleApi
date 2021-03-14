using SeaBattle.Common.Extensions;
using SeaBattle.Common.Types;

namespace SeaBattle.Domain
{
    public class ShotDomainModel
    {
        public Point Point { get; }

        public ShotDomainModel(Point point)
        {
            Point = point.NotNull(nameof(point));
        }
    }
}
