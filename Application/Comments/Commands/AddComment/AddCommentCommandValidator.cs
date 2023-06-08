using FluentValidation;

namespace Application.Comments.Commands.AddComment;

public sealed class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(c => c.Text)
            .MaximumLength(500)
            .WithMessage("Text must not exceed 500 characters!");
    }
}
