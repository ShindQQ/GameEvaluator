using Application.Common.Exceptions;

namespace Presentration.API.Middlewares;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory?.CreateLogger<ExceptionMiddleware>()
            ?? throw new ArgumentNullException(nameof(loggerFactory));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (StatusCodeException ex)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                throw;
            }

            _logger.LogError("{message}", ex.Message);

            await HandleExceptionAsync(context, ex);

            return;
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, StatusCodeException exception)
    {
        context.Response.Clear();
        context.Response.StatusCode = (int)exception.StatusCode;
        context.Response.ContentType = exception.Message;

        await context.Response.WriteAsJsonAsync(new StatusCodeErrorDetails((int)exception.StatusCode,
            exception.Message).Serialize());
    }
}
