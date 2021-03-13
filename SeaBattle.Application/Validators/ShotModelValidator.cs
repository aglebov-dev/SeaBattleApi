using FluentValidation;
using SeaBattle.Application.Abstract;
using SeaBattle.Application.Contracts.Models;

namespace SeaBattle.Application.Validators
{
    public class ShotModelValidator : BaseValidator<ShipsCreationModel>
    {
        public ShotModelValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(model => model.Coordinates)
                .NotEmpty()
                .WithMessage("Invalid coordinate data");
        }
    }
}
