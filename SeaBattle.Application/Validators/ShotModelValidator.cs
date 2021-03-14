using FluentValidation;
using SeaBattle.Application.Contracts.Models;
using SeaBattle.Common.DataValidation;

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
