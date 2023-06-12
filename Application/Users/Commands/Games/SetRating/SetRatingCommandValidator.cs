using FluentValidation;

namespace Application.Users.Commands.Games.SetRating;

public sealed class SetRatingCommandValidator : AbstractValidator<SetRatingCommand>
{
    public SetRatingCommandValidator()
    {
        RuleFor(c => c.Rating)
            .NotEmpty()
            .WithMessage("Rating must not be empty!")
            .GreaterThanOrEqualTo(1)
            .WithMessage("Rating must be at least set to 1!")
            .LessThanOrEqualTo(10)
            .WithMessage("Rating max value is 10!");
    }
}
