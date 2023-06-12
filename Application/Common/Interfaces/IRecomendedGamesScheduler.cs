namespace Application.Common.Interfaces;

public interface IScheduler
{
    Task SendRecomendedGamesAsync(CancellationToken cancellationToken);

    Task UnbanUsersAsync(CancellationToken cancellationToken);
}
