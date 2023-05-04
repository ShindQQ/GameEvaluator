using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Platforms.Commands.UpdateCommand;

public sealed class UpdatePlatformCommandValidator : AbstractValidator<UpdatePlatformCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdatePlatformCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(c => c.Name)
            .MinimumLength(3)
            .WithMessage("Name must be longer than 3 characters!")
            .MaximumLength(20)
            .WithMessage("Name must not exceed 20 characters!")
            .MustAsync(BeUniqueName).WithMessage("Company with such name already exists");

        RuleFor(c => c.Description)
            .MinimumLength(20)
            .WithMessage("Description must be bigger than 20 characters!")
            .MaximumLength(200)
            .WithMessage("Description must not exceed 200 characters!");
    }

    public async Task<bool> BeUniqueName(
        UpdatePlatformCommand command,
        string name,
        CancellationToken cancellationToken)
        => await _context.Platforms
        .Where(platforms => platforms.Id != command.Id)
        .AllAsync(platforms => !platforms.Name.Equals(name), cancellationToken);
}
