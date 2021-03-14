using SeaBattle.Common.Types;
using SeaBattle.Domain;
using SeaBattle.Domain.Abstract;
using SeaBattle.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SeaBattle.Application.InternalServices
{
    // TODO Think about memory
    public class CoordinatesParser : ICoordinatesParser
    {
        private static LetterNumbersConverter converter = new LetterNumbersConverter();
        private static Regex shipCoordinatesRegex = new Regex(@"^\d+[A-Za-z]+\s+\d+[A-Za-z]+$", RegexOptions.Compiled);
        private static Regex pointRegex = new Regex(@"^(\d+)([A-Za-z]+)$", RegexOptions.Compiled);
        private static Regex spaceRegex = new Regex(@"\s+", RegexOptions.Compiled);

        private readonly DataValidationException _defaultException;

        public CoordinatesParser()
        {
            _defaultException = new DataValidationException("Incorect coordinates.");
        }

        public IReadOnlyCollection<ShipDomainModel> ParseShipsCoordinates(string text)
        {
            if (text != null)
            {
                text = spaceRegex.Replace(text, " ").Trim();
            }

            if (!ValidateShipsCoordinates(text))
            {
                throw _defaultException;
            }

            List<ShipDomainModel> results = new List<ShipDomainModel>();
            string[] pairs = text.Split(',');
            foreach (string pair in pairs)
            {
                string[] parts = pair.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                (int x1, int y1) = GetPoint(parts[0]);
                (int x2, int y2) = GetPoint(parts[1]);

                Point startPoint = new Point(Math.Min(x1, x2), Math.Min(y1, y2));
                Point endPoint = new Point(Math.Max(x1, x2), Math.Max(y1, y2));

                results.Add(new ShipDomainModel(default, startPoint, endPoint, Enumerable.Empty<ShotDomainModel>()));
            }

            return results.AsReadOnly();
        }

        public ShotDomainModel ParseCoordinate(string text)
        {
            if (text != null)
            {
                text = spaceRegex.Replace(text, " ").Trim();
            }

            if (!ValidateCoordinate(text))
            {
                throw _defaultException;
            }

            (int x, int y) = GetPoint(text);

            return new ShotDomainModel(new Point(x, y));
        }

        private bool ValidateCoordinate(string text)
        {
            return !string.IsNullOrWhiteSpace(text) && pointRegex.IsMatch(text);
        }

        private bool ValidateShipsCoordinates(string text)
        {
            return text != null && text
                .Split(',')
                .Select(s => s.Trim())
                .All(s => !string.IsNullOrWhiteSpace(s) && shipCoordinatesRegex.IsMatch(s));
        }

        private (int x, int y) GetPoint(string data)
        {
            Match match = pointRegex.Match(data);
            int x = int.Parse(match.Groups[1].Value) - 1;
            int y = converter.GetColunmNumber(match.Groups[2].Value);

            return (x, y);
        }
    }
}
