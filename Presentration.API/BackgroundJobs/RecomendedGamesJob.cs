using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Users.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Presentration.API.Options;

namespace Presentration.API.BackgroundJobs;

public sealed class RecomendedGamesJob : BackgroundService
{
    private readonly PeriodicTimer _periodicTimer;

    private readonly IServiceProvider _serviceProvider;

    private readonly RecommendedGamesJobOptions _recommendedGamesOptions;

    public RecomendedGamesJob(
        IServiceProvider serviceProvider,
        IOptions<RecommendedGamesJobOptions> emailOptions)
    {
        _recommendedGamesOptions = emailOptions.Value;
        _periodicTimer = new(TimeSpan.FromDays(_recommendedGamesOptions.TimerValue));
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (await _periodicTimer.WaitForNextTickAsync(cancellationToken) &&
            !cancellationToken.IsCancellationRequested)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var users = await (await userRepository.GetAsync())
                .Where(user => user.Roles.Any(role => role.Name == RoleType.User.ToString()))
                .ToListAsync(cancellationToken);

            Parallel.ForEach(users, async user =>
            {
                var games = await mediator.Send(new RecomendedGamesQuery
                {
                    UserId = user.Id,
                    AmountOfGames = _recommendedGamesOptions.AmountOfGames
                }, cancellationToken);

                await emailService.SendEmailAsync(user, games);
            });
        }
    }
}
