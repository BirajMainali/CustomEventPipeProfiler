using CustomEventPipeProfiler.Data;
using CustomEventPipeProfiler.Extensions;

namespace CustomEventPipeProfiler.Accumulators;

public class CpuUsageAccumulator : UsageAccumulator
{
    public override void Accumulate(int processId, IDictionary<string, object> payload)
    {
        if (!payload.IsCpuUsage()) return;
        
        Channel.Writer.TryWrite(new MetricPoint(
            timestamp: DateTime.Now,
            resource: "CPU",
            usage: payload.GetMean(),
            measure: "%"));
    }
}