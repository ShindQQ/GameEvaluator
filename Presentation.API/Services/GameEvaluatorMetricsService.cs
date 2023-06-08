using System.Diagnostics.Metrics;

namespace Presentration.API.Services;

public sealed class GameEvaluatorMetricsService
{
    private readonly Meter _meter;

    public Counter<long> PayloadCounter { get; }

    public Counter<int> RequestCounter { get; }

    public Histogram<double> ConfidenceHistogram { get; }

    public GameEvaluatorMetricsService()
    {
        _meter = new Meter("ComputerVision");

        PayloadCounter = _meter.CreateCounter<long>("PayloadCounter", "bytes");
        RequestCounter = _meter.CreateCounter<int>("RequestCounter");
        ConfidenceHistogram = _meter.CreateHistogram<double>("ConfidenceHistogram");
    }
}
