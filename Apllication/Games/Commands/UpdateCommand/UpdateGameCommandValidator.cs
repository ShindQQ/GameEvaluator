using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Games.Commands.UpdateCommand;

public sealed class UpdateGameCommandValidator : AbstractValidator<UpdateGameCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateGameCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(c => c.Name)
            .MinimumLength(3)
            .WithMessage("Name must be longer than 3 characters!")
            .MaximumLength(20)
            .WithMessage("Name must not exceed 20 characters!")
            .MustAsync(BeUniqueName).WithMessage("Game with such name already exists");

        RuleFor(c => c.Description)
            .MinimumLength(20)
            .WithMessage("Description must be bigger than 20 characters!")
            .MaximumLength(200)
            .WithMessage("Description must not exceed 200 characters!");
    }

    public async Task<bool> BeUniqueName(
        UpdateGameCommand command,
        string name,
        CancellationToken cancellationToken)
        => await _context.Games
          .Where(game => game.Id != command.Id)
          .AllAsync(game => !game.Name.Equals(name), cancellationToken);
}
