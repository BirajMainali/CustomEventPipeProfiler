using CustomEventPipeProfiler.Accumulators;
using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;
using System.Diagnostics.Tracing;

namespace CustomEventPipeProfiler.Diagnostics;

public class DiagnosticEngine
{
    public readonly List<UsageAccumulator> Accumulators = [];

    public DiagnosticEngine AttachAccumulator(UsageAccumulator acc)
    {
        Accumulators.Add(acc);
        return this;
    }

    public void Start(int processId)
    {
        Task.Run(() =>
        {
            var providers = new List<EventPipeProvider>
            {
                new EventPipeProvider(
                    name: "System.Runtime",
                    eventLevel: EventLevel.Informational,
                    keywords: -1,
                    new Dictionary<string, string>
                    {
                        { "EventCounterIntervalSec", "1" }
                    })
            };

            var client = new DiagnosticsClient(processId);
            using var session = client.StartEventPipeSession(providers);
            var source = new EventPipeEventSource(session.EventStream);

            source.Dynamic.All += (data) =>
            {
                if (!data.EventName.Equals("EventCounters")) return;

                foreach (var acc in Accumulators)
                {
                    var payload = (IDictionary<string, object>)((IDictionary<string, object>)data.PayloadValue(0))["Payload"];
                    acc.Accumulate(processId, payload);
                }
            };

            source.Process();
        });
    }
}