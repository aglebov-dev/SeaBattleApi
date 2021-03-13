using SeaBattle.Common.Types;
using SeaBattle.Domain;
using System;
using System.Linq;
using Xunit;

namespace SeaBattle.Tests
{
    public class ShipDomainModelTests
    {
        [Theory]
        [InlineData(0, 0, 2, 2, 9)]
        [InlineData(1, 2, 4, 3, 8)]
        [InlineData(0, 0, 0, 0, 1)]
        [InlineData(10, 10, 10, 10, 1)]
        public void MaxHealthTest(int x1, int y1, int x2, int y2, int maxHealth)
        {
            ShipDomainModel model = new ShipDomainModel
            (
                id: 1,
                start: new Point(x1, y1),
                end: new Point(x2, y2),
                shots: Array.Empty<ShotDomainModel>()
            );

            Assert.Equal(maxHealth, model.MaxHealth);
        }

        [Fact]
        public void HealthTest()
        {
            var shots = Enumerable
                .Range(0, 5)
                .Select(n => new ShotDomainModel(new Point(n, n)))
                .ToArray();

            ShipDomainModel model = new ShipDomainModel
            (
                id: 1,
                start: new Point(0, 0),
                end: new Point(4, 4),
                shots: shots
            );

            Assert.Equal(20, model.Health);
        }
    }
}
