using CustomEventPipeProfiler.Accumulators;
using CustomEventPipeProfiler.Diagnostics;
using CustomEventPipeProfiler.Providers;
using Spectre.Console;

var currentProcesses = ProcessProvider.GetCurrentProcesses();

var (Id, Path) = AnsiConsole.Prompt(
new SelectionPrompt<(int Id, string Path)>()
    .Title("Choose the process you want to monitor.")
    .AddChoices(currentProcesses));

AnsiConsole.MarkupLine($"[bold yellow]Selected Process:[/] [green]{Path}[/] (PID: [blue]{Id}[/])");

var diagnosticEngine = new DiagnosticEngine()
    .AttachAccumulator(new CpuUsageAccumulator())
    .AttachAccumulator(new MemoryUsageAccumulator());

diagnosticEngine.Start(Id);

AnsiConsole.MarkupLine(
    $"[bold purple]{"Timestamp",-30} {"Resource",-10} {"Usage",10} {"Measure",15}[/]"
);

await Parallel.ForEachAsync(diagnosticEngine.Accumulators, async (accumulator, ct) =>
{
    var reader = accumulator.Reader;
    while (await reader.WaitToReadAsync(ct))
    {
        while (reader.TryRead(out var metric))
        {
            AnsiConsole.MarkupLine(
                $"[purple]{metric.Timestamp,-30}[/] " +
                $"[yellow]{metric.Resource,-10}[/] " +
                $"[red]{metric.Usage,10:F2}[/] " +
                $"[blue]{metric.Measure,15}[/]"
            );

        }
    }
});