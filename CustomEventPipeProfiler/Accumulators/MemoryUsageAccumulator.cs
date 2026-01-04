using CustomEventPipeProfiler.Data;
using System.Diagnostics;

namespace CustomEventPipeProfiler.Accumulators;

public class MemoryUsageAccumulator : UsageAccumulator
{
    private DateTime _lastRun = DateTime.MinValue;

    public override void Accumulate(int processId, IDictionary<string, object> payload)
    {
        if ((DateTime.Now - _lastRun).TotalMilliseconds < 1000) return;
        _lastRun = DateTime.Now;

        var process = Process.GetProcessById(processId);

        process.Refresh();
        double consumption = process.PrivateMemorySize64 / 1024.0 / 1024.0;

        Channel.Writer.TryWrite(new MetricPoint(
            timestamp: DateTime.Now,
            resource: "Memory",
            usage: consumption,
            measure: "MB"));
    }
}