using Apllication.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Apllication.Companies.Commands.CreateCommand;

public sealed class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCompanyCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Name must not be empty!")
            .MinimumLength(3)
            .WithMessage("Name must be longer than 3 characters!")
            .MaximumLength(20)
            .WithMessage("Name must not exceed 20 characters!")
            .MustAsync(BeUniqueName).WithMessage("Company with such name already exists");

        RuleFor(c => c.Description)
            .NotEmpty()
            .WithMessage("Description must not be empty!")
            .MinimumLength(20)
            .WithMessage("Description must be bigger than 20 characters!")
            .MaximumLength(200)
            .WithMessage("Description must not exceed 200 characters!");
    }

    public async Task<bool> BeUniqueName(
        string name,
        CancellationToken cancellationToken)
        => await _context.Companies.AllAsync(company => !company.Name.Equals(name), cancellationToken);
}
