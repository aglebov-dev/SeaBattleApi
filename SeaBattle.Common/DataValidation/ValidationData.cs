using System.Collections.Generic;

namespace SeaBattle.Common.DataValidation
{
    public class ValidationData
    {
        public bool Success { get; }
        public IReadOnlyCollection<string> Errors { get; }

        public ValidationData(bool success, IReadOnlyCollection<string> errors)
        {
            Success = success;
            Errors = errors;
        }
    }
}
