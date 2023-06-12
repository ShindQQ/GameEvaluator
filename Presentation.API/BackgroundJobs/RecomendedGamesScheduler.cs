using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Users.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Presentration.API.Options;

namespace Presentration.API.BackgroundJobs;

public sealed class Scheduler : IScheduler
{
    private readonly RecommendedGamesJobOptions _recommendedGamesOptions;

    private readonly IUserRepository _userRepository;

    private readonly IMediator _mediator;

    private readonly IEmailService _emailService;

    public Scheduler(
       IOptions<RecommendedGamesJobOptions> recommendedGamesOptions,
       IUserRepository userRepository,
       IMediator mediator,
       IEmailService emailService)
    {
        _recommendedGamesOptions = recommendedGamesOptions.Value;
        _userRepository = userRepository;
        _mediator = mediator;
        _emailService = emailService;
    }

    public async Task SendRecomendedGamesAsync(CancellationToken cancellationToken)
    {
        var users = await (await _userRepository.GetAsync())
            .Where(user => user.Roles.Any(role => role.Name == RoleType.User.ToString()))
            .ToListAsync(cancellationToken);

        foreach (var user in users)
        {
            var games = await _mediator.Send(new RecomendedGamesQuery
            {
                UserId = user.Id,
                AmountOfGames = _recommendedGamesOptions.AmountOfGames
            }, cancellationToken);

            await _emailService.SendEmailAsync(user, games);
        }
    }

    public async Task UnbanUsersAsync(CancellationToken cancellationToken)
    {
        var users = await (await _userRepository.GetAsync())
           .Where(user => user.Roles.Any(role => role.Name == RoleType.User.ToString()))
           .ToListAsync(cancellationToken);

        foreach (var user in users)
        {
            if (user.BanState is not null && user.BanState.BannedTo.Equals(DateTime.Now))
                user.UnBan();
        }
    }
}
