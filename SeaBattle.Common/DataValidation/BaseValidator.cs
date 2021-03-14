using FluentValidation;
using FluentValidation.Results;
using System;
using System.Linq;

namespace SeaBattle.Common.DataValidation
{
    public abstract class BaseValidator<TModel> : AbstractValidator<TModel>, IModelValidator
    {
        public bool CanValiate<T>(T model)
        {
            return model is TModel;
        }

        public ValidationData Validate<T>(T model)
        {
            if (model is TModel data)
            {
                ValidationResult result = base.Validate(data);
                string[] errors = result.Errors.Select(e => e.ErrorMessage).ToArray();

                return new ValidationData(result.IsValid, errors);
            }

            throw new ApplicationException("Unexpected type of model for validation.");
        }
    }
}
