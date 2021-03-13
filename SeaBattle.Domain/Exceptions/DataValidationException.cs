using System;
using System.Collections.Generic;

namespace SeaBattle.Domain.Exceptions
{
    public class DataValidationException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public DataValidationException(params string[] errors)
        {
            Errors = errors;
        }

        public DataValidationException(IEnumerable<string> errors)
        {
            Errors = errors;
        }
    }
}
