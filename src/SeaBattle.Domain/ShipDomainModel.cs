using SeaBattle.Common.Extensions;
using SeaBattle.Common.Types;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SeaBattle.Domain
{
    public class ShipDomainModel
    {
        public long Id { get; }
        public Point Start { get; }
        public Point End { get; }
        public int Health { get; }
        public int MaxHealth { get; }
        public IReadOnlyCollection<ShotDomainModel> Shots { get; }

        public ShipDomainModel(long id, Point start, Point end, IEnumerable<ShotDomainModel> shots)
        {
            AssertExtensions.NotNull(start, nameof(start));
            AssertExtensions.NotNull(end, nameof(end));
            AssertExtensions.NotNull(shots, nameof(shots));

            Id     = id;
            Start  = start;
            End    = end;
            Shots  = shots.ToImmutableList();
            MaxHealth = (Math.Abs(start.X - end.X) + 1) * (Math.Abs(start.Y - end.Y) + 1);
            Health = GetHealth(shots);
        }

        private int GetHealth(IEnumerable<ShotDomainModel> shots)
        {
            var actualShots = shots
                .Where(s => s.Point.X >= Start.X && s.Point.Y >= Start.Y)
                .Where(s => s.Point.X <= End.X && s.Point.Y <= End.Y)
                .ToArray();

            return Math.Max(0, MaxHealth - actualShots.Length);
        }

        public ShipDomainModel With(ShotDomainModel shot)
        {
            return new ShipDomainModel(Id, Start, End, Shots.Append(shot));
        }

        public ShipDomainModel With(long id)
        {
            return new ShipDomainModel(id, Start, End, Shots);
        }
    }
}
