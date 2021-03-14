using SeaBattle.Application.InternalServices;
using SeaBattle.Domain.Exceptions;
using Xunit;

namespace SeaBattle.Tests
{
    public class LetterNumbersConverterTests
    {
        [Theory]
        [InlineData("A", 0)]
        [InlineData("Z", 25)]
        [InlineData("AA", 26)]
        [InlineData("AZ", 51)]
        [InlineData("AAA", 702)]
        [InlineData("ZZZZZ", 12356629)]
        public void GetColunmNumber_ThenDataIsCorrect_ReturnNumber(string colunm, int result)
        {
            LetterNumbersConverter converter = new LetterNumbersConverter();

            int number = converter.GetColunmNumber(colunm);

            Assert.Equal(result, number);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("78")]
        [InlineData("*&#")]
        [InlineData("🙂")]
        public void GetColunmNumber_ThenDataInorrect_ThrowException(string colunm)
        {
            LetterNumbersConverter converter = new LetterNumbersConverter();

            Assert.Throws<DataValidationException>(() => converter.GetColunmNumber(colunm));
        }

        [Fact]
        public void GetColunmNumber_ToMuchValue_ThrowException()
        {
            LetterNumbersConverter converter = new LetterNumbersConverter();
            var colunm = "FXSHRXY"; // int.MaxValue + 1

            Assert.Throws<DataValidationException>(() => converter.GetColunmNumber(colunm));
        }
    }
}
