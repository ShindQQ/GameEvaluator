using FluentValidation;

namespace Application.Games.Commands.Comments.AddCommentToComment;

public sealed class AddCommentToCommentCommandValidator : AbstractValidator<AddCommentToCommentCommand>
{
    public AddCommentToCommentCommandValidator()
    {
        RuleFor(c => c.Text)
            .MaximumLength(500)
            .WithMessage("Text must not exceed 500 characters!");
    }
}
