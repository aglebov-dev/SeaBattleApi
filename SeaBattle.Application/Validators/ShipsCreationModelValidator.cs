using FluentValidation;
using SeaBattle.Application.Abstract;
using SeaBattle.Application.Contracts.Models;

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
