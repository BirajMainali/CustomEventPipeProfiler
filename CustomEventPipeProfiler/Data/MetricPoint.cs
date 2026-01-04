namespace CustomEventPipeProfiler.Data;

public struct MetricPoint
{
    public MetricPoint(DateTimeOffset timestamp, string resource, double usage, string measure)
    {
        Timestamp = timestamp;
        Resource = resource;
        Usage = usage;
        Measure = measure;
    }

    public DateTimeOffset Timestamp { get; }
    public string Resource { get; }
    public double Usage { get; }
    public string Measure { get; }
}