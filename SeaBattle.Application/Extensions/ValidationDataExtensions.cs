using SeaBattle.Application.Abstract;
using SeaBattle.Domain.Exceptions;

namespace SeaBattle.Application.Extensions
{
    public static class ValidationDataExtensions
    {
        public static void ThrowIfHasErrors(this ValidationData result)
        {
            if (!result.Success)
            {
                throw new DataValidationException(result.Errors);
            }
        }
    }
}
