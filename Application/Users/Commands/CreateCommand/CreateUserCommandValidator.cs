using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.CreateCommand;

public sealed class UpdateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Name must not be empty!")
            .MinimumLength(3)
            .WithMessage("Name must be longer than 3 characters!")
            .MaximumLength(20)
            .WithMessage("Name must not exceed 20 characters!")
            .MustAsync(BeUniqueName)
            .WithMessage("User with such name already exists");

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("Email must not be empty!")
            .MinimumLength(10)
            .WithMessage("Email must be longer than 10 characters!")
            .EmailAddress()
            .WithMessage("This email is not valid!")
            .MaximumLength(30)
            .WithMessage("Email must not exceed 30 characters!")
            .MustAsync(BeUniqueEmail)
            .WithMessage("User with such email already exists");

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("Password must not be empty!")
            .MinimumLength(6)
            .WithMessage("Password must be bigger than 6 characters!")
            .MaximumLength(10)
            .WithMessage("Password must not exceed 10 characters!");
    }

    public async Task<bool> BeUniqueName(
        string name,
        CancellationToken cancellationToken)
        => await _context.Users
        .AllAsync(user => !user.Name.Equals(name), cancellationToken);

    public async Task<bool> BeUniqueEmail(
        string email,
        CancellationToken cancellationToken)
        => await _context.Users
        .AllAsync(user => !user.Email.Equals(email), cancellationToken);
}
