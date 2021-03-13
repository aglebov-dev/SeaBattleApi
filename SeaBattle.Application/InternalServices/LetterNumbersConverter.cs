using SeaBattle.Domain.Exceptions;
using System.Linq;

namespace SeaBattle.Application.InternalServices
{
    public class LetterNumbersConverter
    {
        public int GetColunmNumber(string column)
        {
            if (!column.All(char.IsLetter))
            {
                throw new DataValidationException("Incorrect input.");
            }

            column = column.ToUpperInvariant();
            int number = 0;
            int pow = 1;
            for (int i = column.Length - 1; i >= 0; i--)
            {
                number += (column[i] - 'A' + 1) * pow;
                pow *= 26;
            }

            return number - 1;
        }
    }
}
