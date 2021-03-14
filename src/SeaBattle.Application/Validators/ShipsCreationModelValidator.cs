using FluentValidation;
using SeaBattle.Application.Contracts.Models;
using SeaBattle.Common.DataValidation;

namespace SeaBattle.Application.Validators
{
    public class ShipsCreationModelValidator : BaseValidator<ShipsCreationModel>
    {
        public ShipsCreationModelValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(model => model.Coordinates)
                .NotEmpty()
                .WithMessage("Invalid coordinate data");
        }
    }
}
