namespace Application.Common.Interfaces;

public interface IRecomendedGamesScheduler
{
    Task SendRecomendedGamesAsync(CancellationToken cancellationToken);
}
