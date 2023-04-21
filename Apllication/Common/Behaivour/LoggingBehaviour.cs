using Apllication.Common.Interfaces;
using Apllication.Common.Interfaces.Repositories;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Apllication.Common.Behaivour;

public sealed class LoggingBehaviour<TRequest>
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger _logger;

    private readonly IUserService _userService;

    private readonly IUserRepository _userRepository;

    public LoggingBehaviour(
        IUserService userService,
        ILogger logger,
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
            var user = await _userRepository.GetByIdAsync(userId);

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
