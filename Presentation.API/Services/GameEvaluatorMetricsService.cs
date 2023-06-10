using System.Diagnostics.Metrics;

namespace Presentration.API.Services;

public static class GameEvaluatorMetricsService
{
    public static readonly Meter Meter = new ("GameEvaluatorMeter");

    public static Counter<int> RequestCounter { get; } = Meter.CreateCounter<int>("app.request_counter");
}
