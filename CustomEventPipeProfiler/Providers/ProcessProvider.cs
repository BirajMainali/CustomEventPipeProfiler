using System.Diagnostics;
using Microsoft.Diagnostics.NETCore.Client;

namespace CustomEventPipeProfiler.Providers;

public static class ProcessProvider
{
    public static IEnumerable<(int Id, string Path)> GetCurrentProcesses()
    {
        var currentProcessIds = DiagnosticsClient.GetPublishedProcesses();

        foreach (var processId in currentProcessIds)
        {
            using var process = Process.GetProcessById(processId);
            var path = process.MainModule?.FileName?.Split("\\").Last();
            yield return (process.Id, path ?? "Unknown Path");
        }
    }
}