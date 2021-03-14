using FluentValidation;
using SeaBattle.Application.Contracts.Models;
using SeaBattle.Common.DataValidation;

namespace SeaBattle.Application.Validators
{
    public class BoardCreationModelValidator : BaseValidator<GameCreationModel>
    {
        public BoardCreationModelValidator()
        {
            RuleFor(model => model.Size)
                .GreaterThan(0)
                .WithMessage("The board size must be greater than 0");
        }
    }
}
