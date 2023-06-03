using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Common.Behaviour;

public sealed class PerformanceBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Stopwatch _timer;

    private readonly ILogger<TRequest> _logger;

    private readonly IUserService _userService;

    private readonly IUserRepository _userRepository;

    private const int CriticalTime = 500;

    public PerformanceBehaviour(
        ILogger<TRequest> logger,
        IUserService userService,
        IUserRepository repository)
    {
        _timer = new Stopwatch();
        _logger = logger;
        _userService = userService;
        _userRepository = repository;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > CriticalTime)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _userService.UserId;
            var userName = string.Empty;

            if (userId != null)
            {
                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

                if (user != null)
                    userName = user.Name;
            }

            _logger.LogInformation(
                "Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                requestName,
                elapsedMilliseconds,
                userId,
                userName,
                request);
        }

        return response;
    }
}
