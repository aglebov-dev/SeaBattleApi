using SeaBattle.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace SeaBattle.Application.InternalServices
{
    public class LetterNumbersConverter
    {
        private static Regex validationRegex = new Regex("^[A-Za-z]+$", RegexOptions.Compiled);

        /// <summary>
        /// Gets the ordinal of the column when numbering starting from 0.
        /// </summary>
        /// <param name="column">Column number in alphabetical form.</param>
        /// <returns>Number of column.</returns>
        public int GetColunmNumber(string column)
        {
            if (column is null || !validationRegex.IsMatch(column))
            {
                throw new DataValidationException($"Incorrect colunm name. Value: '{column}'.");
            }

            column = column.ToUpperInvariant();
            long number = 0;
            long pow = 1;
            for (int i = column.Length - 1; i >= 0; i--)
            {
                number += (column[i] - 'A' + 1) * pow;
                pow *= 26;

                if ((number -1 ) > int.MaxValue)
                {
                    throw new DataValidationException($"Too much value. Value: '{column}'.");
                }
            }

            return (int)(number - 1);
        }
    }
}
