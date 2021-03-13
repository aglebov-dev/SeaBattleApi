using SeaBattle.Application.InternalServices;
using SeaBattle.Domain;
using SeaBattle.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SeaBattle.Tests
{
    public class CoordinatesParserTests
    {
        private CoordinatesParser Create()
        {
            return new CoordinatesParser();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("1 A")]
        [InlineData("1245")]
        [InlineData("AA33")]
        public void ParseShotCoordinate_IncorrectData_ThrowException(string text)
        {
            CoordinatesParser parser = Create();

            Assert.Throws<DataValidationException>(() => parser.ParseCoordinate(text));
        }

        [Theory]
        [InlineData("1A", 0, 0)]
        [InlineData("5z", 4, 25)]
        [InlineData("   15AZ", 14, 51)]
        [InlineData("45WFS  ", 44, 15722)]
        public void ParseShotCoordinate_CorrectData_ReturnResult(string text, int x, int y)
        {
            CoordinatesParser parser = Create();

            ShotDomainModel result = parser.ParseCoordinate(text);

            Assert.NotNull(result);
            Assert.Equal(x, result.Point.X);
            Assert.Equal(y, result.Point.Y);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("15AZ")]
        [InlineData("1A 1A 1A")]
        [InlineData("1A 1A  ,,,, 2B 2B")]
        [InlineData(",")]
        public void ParseShipsCoordinates_IncorrectData_ThrowException(string text)
        {
            CoordinatesParser parser = Create();

            Assert.Throws<DataValidationException>(() => parser.ParseShipsCoordinates(text));
        }

        [Theory]
        [InlineData("1A 1A", 1)]
        [InlineData("1A 1A, 2B 4R", 2)]
        [InlineData("1A 1A, 2B    4R ", 2)]
        [InlineData("1A 1A, 2B 2B, 9Z 9Z", 3)]
        public void ParseShipsCoordinates_CorrectData_ReturnResults(string text, int count)
        {
            CoordinatesParser parser = Create();

            IReadOnlyCollection<ShipDomainModel> results = parser.ParseShipsCoordinates(text);

            Assert.NotNull(results);
            Assert.Equal(count, results.Count);
        }

        [Theory]
        [InlineData("1A 1A", 0, 0, 0, 0)]
        [InlineData("5Z 5z", 4, 25, 4, 25)]
        [InlineData("   15AZ  1A", 14, 51, 0, 0)]
        public void ParseShipsCoordinates_CorrectData_ReturnCorrectResults(string text, int x1, int y1, int x2, int y2)
        {
            CoordinatesParser parser = Create();

            IReadOnlyCollection<ShipDomainModel> results = parser.ParseShipsCoordinates(text);

            Assert.NotNull(results);
            Assert.Single(results);

            ShipDomainModel p = results.First();
            Assert.Equal(Math.Min(x1, x2), p.Start.X);
            Assert.Equal(Math.Min(y1, y2), p.Start.Y);
            Assert.Equal(Math.Max(x1, x2), p.End.X);
            Assert.Equal(Math.Max(y1, y2), p.End.Y);
        }
    }
}
