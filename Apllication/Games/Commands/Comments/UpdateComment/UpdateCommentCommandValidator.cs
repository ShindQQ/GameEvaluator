using FluentValidation;

namespace Application.Games.Commands.Comments.UpdateComment;

public sealed class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(c => c.Text)
            .MaximumLength(500)
            .WithMessage("Text must not exceed 500 characters!");
    }
}
