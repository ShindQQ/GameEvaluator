using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviour;

public sealed class LoggingBehaviour<TRequest>
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;

    private readonly IUserService _userService;

    private readonly IUserRepository _userRepository;

    public LoggingBehaviour(
        IUserService userService,
        ILogger<TRequest> logger,
        IUserRepository repository)
    {
        _userService = userService;
        _logger = logger;
        _userRepository = repository;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
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
            "Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName,
            userId,
            userName,
            request);
    }
}
