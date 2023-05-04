using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.UpdateCommand;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(c => c.Name)
            .MinimumLength(3)
            .WithMessage("Name must be longer than 3 characters!")
            .MaximumLength(20)
            .WithMessage("Name must not exceed 20 characters!")
            .MustAsync(BeUniqueName).WithMessage("User with such name already exists");

        RuleFor(c => c.Email)
            .MinimumLength(10)
            .WithMessage("Email must be longer than 10 characters!")
            .EmailAddress()
            .WithMessage("This email is not valid!")
            .MaximumLength(30)
            .WithMessage("Email must not exceed 30 characters!")
            .MustAsync(BeUniqueEmail)
            .WithMessage("User with such email already exists");
    }

    public async Task<bool> BeUniqueName(
        UpdateUserCommand command,
        string name,
        CancellationToken cancellationToken)
        => await _context.Users
        .Where(user => user.Id != command.Id)
        .AllAsync(user => !user.Name.Equals(name), cancellationToken);

    public async Task<bool> BeUniqueEmail(
        UpdateUserCommand command,
        string email,
        CancellationToken cancellationToken)
        => await _context.Users
        .Where(user => user.Id != command.Id)
        .AllAsync(user => !user.Email.Equals(email), cancellationToken);
}
