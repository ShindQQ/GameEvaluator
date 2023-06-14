using Application.Common.Exceptions;
using Presentration.API.Services;
using System.Diagnostics;

namespace Presentration.API.Middlewares;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionMiddleware> _logger;

    private readonly ActivitySource _source;

    public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory?.CreateLogger<ExceptionMiddleware>()
            ?? throw new ArgumentNullException(nameof(loggerFactory));
        _source = new ActivitySource(GameEvaluatorMetricsService.SourceName);
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

    private async Task HandleExceptionAsync(HttpContext context, StatusCodeException exception)
    {
        using var activity = _source.StartActivity(GameEvaluatorMetricsService.SourceName, ActivityKind.Internal)!;

        context.Response.Clear();
        context.Response.StatusCode = (int)exception.StatusCode;
        context.Response.ContentType = exception.Message;

        var details = new StatusCodeErrorDetails((int)exception.StatusCode,
            exception.Message).Serialize();

        await context.Response.WriteAsJsonAsync(details);

        GameEvaluatorMetricsService.RequestCounter.Add(1,
            new("Action", nameof(HandleExceptionAsync)),
            new("ExceptionMiddleware", nameof(ExceptionMiddleware)));

        activity.AddEvent(new ActivityEvent("Thrown exception", tags: new ActivityTagsCollection(new[]
        {
            KeyValuePair.Create<string, object?>("Exception", details)
        })));

        activity.SetTag("otel.status_code", (int)exception.StatusCode);
        activity.SetTag("otel.status_description", "Exception handled");
    }
}
