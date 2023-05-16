using FluentValidation;

namespace Application.Games.Commands.Comments.AddComment;

public sealed class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(c => c.Text)
            .MaximumLength(500)
            .WithMessage("Text must not exceed 500 characters!");
    }
}
