using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using Apllication.Users.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Presentration.API.BackgroundJobs;

public sealed class RecomendedGamesJob : BackgroundService
{
    private readonly PeriodicTimer _periodicTimer;

    private readonly IServiceProvider _serviceProvider;

    public RecomendedGamesJob(
        IServiceProvider serviceProvider)
    {
        _periodicTimer = new(TimeSpan.FromDays(1));
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

            foreach (var user in users)
            {
                var games = await mediator.Send(new RecomendedGamesQuery
                {
                    UserId = user.Id,
                    AmountOfGames = 5
                }, cancellationToken);

                await emailService.SendEmailAsync(user, games);
            }
        }
    }
}
