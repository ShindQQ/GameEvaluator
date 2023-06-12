using Presentration.API.Services;
using System.Diagnostics;

namespace Presentation.API.Middlewares;

public sealed class RequestsMiddleware
{
    private readonly RequestDelegate _next;

    private const string SourceName = "GameEvaluator";

    private readonly ActivitySource _source;

    public RequestsMiddleware(RequestDelegate next)
    {
        _next = next;
        _source = new ActivitySource(SourceName);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using var activity = _source.StartActivity(SourceName, ActivityKind.Internal)!;

        var originalBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        await _next(context);

        var responseBody = await FormatResponseAsync(context.Response);

        await responseBodyStream.CopyToAsync(originalBodyStream);

        var response = context.Response;
        var controllerName = context.GetRouteData().Values["controller"]?.ToString();
        var actionName = context.GetRouteData().Values["action"]?.ToString();

        GameEvaluatorMetricsService.RequestCounter.Add(1,
            new("Action", actionName),
            new("Controller", controllerName));

        activity.AddEvent(new ActivityEvent($"Activity {controllerName} {actionName}", tags: new ActivityTagsCollection(new[]
        {
            KeyValuePair.Create<string, object?>($"Result from {controllerName} {actionName}", responseBody)
        })));

        activity.SetTag("otel.status_code", response.StatusCode);
        activity.SetTag("otel.status_description", responseBody);
    }

    private static async Task<string> FormatResponseAsync(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);

        var text = await new StreamReader(response.Body).ReadToEndAsync();

        response.Body.Seek(0, SeekOrigin.Begin);

        return $"{response.StatusCode}: {text}";
    }
}
