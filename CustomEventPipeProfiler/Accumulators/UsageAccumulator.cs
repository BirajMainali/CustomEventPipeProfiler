using System.Threading.Channels;
using CustomEventPipeProfiler.Data;

namespace CustomEventPipeProfiler.Accumulators;

public abstract class UsageAccumulator
{
    internal readonly Channel<MetricPoint> Channel = System.Threading.Channels.Channel.CreateUnbounded<MetricPoint>();
    public ChannelReader<MetricPoint> Reader => Channel.Reader;

    /// <summary>
    /// Accumulates data for the specified process using the provided payload.
    /// It triggers in every EventPipe event received in 1 second interval.
    /// </summary>
    /// <param name="processId">The identifier of the process for which data is being accumulated.</param>
    /// <param name="payload">A dictionary containing key-value pairs that represent the data to accumulate. Keys are strings identifying the
    /// data fields; values are the corresponding data objects. Cannot be null.</param>
    public abstract void Accumulate(int processId, IDictionary<string, object> payload);
}