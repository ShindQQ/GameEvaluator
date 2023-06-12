using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Commands.UpdateCommand;

public sealed class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCompanyCommandValidator(IApplicationDbContext context)
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
        UpdateCompanyCommand command,
        string name,
        CancellationToken cancellationToken)
        => await _context.Companies
        .Where(company => company.Id != command.Id)
        .AllAsync(company => !company.Name.Equals(name), cancellationToken);
}
