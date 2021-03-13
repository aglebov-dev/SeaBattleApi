using SeaBattle.Common.Types;
using SeaBattle.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace SeaBattle.Domain
{
    public class GameDomainModel
    {
        public long Id { get; }
        public int Size { get; }
        public bool Init { get; }
        public bool Ended { get; set; }

        public GameDomainModel(long id, int size, bool init, bool ended)
        {
            Id = id;
            Size = size;
            Init = init;
            Ended = ended;
        }

        public bool CanAddShips(IEnumerable<ShipDomainModel> ships)
        {
            if (Init || Ended)
            {
                return false;
            }

            var hash = new HashSet<(int x, int y)>();
            foreach (ShipDomainModel ship in ships ?? Enumerable.Empty<ShipDomainModel>())
            {
                if (!DoesContainsPoint(ship.Start) || !DoesContainsPoint(ship.End))
                {
                    throw new DataValidationException("Incorrect data.");
                }

                for (int x = ship.Start.X; x <= ship.End.X; x++)
                {
                    for (int y = ship.Start.Y; y <= ship.End.Y; y++)
                    {
                        if (hash.Contains((x, y)))
                        {
                            return false;
                        }

                        hash.Add((x, y));
                    }
                }
            }

            return true;
        }

        public bool CanTakeShot(ShotDomainModel shot)
        {
            return
                Init &&
                !Ended &&
                DoesContainsPoint(shot.Point);
        }

        private bool DoesContainsPoint(Point point)
        {
            return
                point.X >= 0 &&
                point.Y >= 0 &&
                point.X < Size &&
                point.Y < Size;
        }
    }
}
