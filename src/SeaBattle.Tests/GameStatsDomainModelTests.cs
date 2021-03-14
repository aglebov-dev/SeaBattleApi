using SeaBattle.Common.Types;
using SeaBattle.Domain;
using SeaBattle.Domain.Exceptions;
using System;
using System.Linq;
using Xunit;

namespace SeaBattle.Tests
{
    public class GameStatsDomainModelTests
    {
        [Fact]
        public void CanAddShips_WhenArgIsNull_ReturnTrue()
        {
            var model = new GameDomainModel(id: 7, size: 10, init: false, ended: false);

            var result = model.CanAddShips(default);

            Assert.True(result);
        }

        [Fact]
        public void CanAddShips_WhenArgIsEmptyCollection_ReturnTrue()
        {
            var model = new GameDomainModel(id: 7, size: 10, init: false, ended: false);

            var result = model.CanAddShips(new ShipDomainModel[] { });

            Assert.True(result);
        }

        [Fact]
        public void CanAddShips_WhenShipsNotCross_ReturnTrue()
        {
            var model = new GameDomainModel(id: 7, size: 10, init: false, ended: false);
            var ships = Enumerable.Range(0, 5)
                .Select(n => CreateShip(n, n, n, n));

            var result = model.CanAddShips(ships);

            Assert.True(result);
        }

        [Fact]
        public void CanAddShips_WhenShipsCross_ReturnFalse()
        {
            var model = new GameDomainModel(id: 7, size: 10, init: false, ended: false);
            var ships = new[]
            {
                CreateShip(0, 0, 5, 5),
                CreateShip(5, 5, 9, 9)
            };

            var result = model.CanAddShips(ships);

            Assert.False(result);
        }

        [Theory]
        [InlineData(111, 5)]
        [InlineData(1, 555)]
        public void CanAddShips_WhenShipsOutOfBounds_ThrowExption(int x, int y)
        {
            var model = new GameDomainModel(id: 7, size: 10, init: false, ended: false);
            var ships = new[]
            {
                CreateShip(0, 0, x, y)
            };

            Assert.Throws<DataValidationException>(() => model.CanAddShips(ships));
        }

        [Fact]
        public void CanAddShips_WhenGameAlreadyInit_ReturnFalse()
        {
            var model = new GameDomainModel(id: 7, size: 10, init: true, ended: false);
            var ships = new ShipDomainModel[] { };

            var result = model.CanAddShips(ships);

            Assert.False(result);
        }

        [Fact]
        public void CanAddShips_WhenGameAlreadyEnded_ReturnFalse()
        {
            var model = new GameDomainModel(id: 7, size: 10, init: false, ended: true);
            var ships = new ShipDomainModel[] { };

            var result = model.CanAddShips(ships);

            Assert.False(result);
        }

        private ShipDomainModel CreateShip(int x1, int y1, int x2, int y2)
        {
            var start = new Point(x1, y1);
            var end = new Point(x2, y2);

            return new ShipDomainModel(0, start, end, Array.Empty<ShotDomainModel>());
        }
    }
}
