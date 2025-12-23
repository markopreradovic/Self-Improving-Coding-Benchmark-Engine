using System.Text.Json;

namespace Benchmark.Engine.Metrics;

public static class MetricsLogger
{
    private static readonly string _logFile = "metrics_log.json";

    public static async Task LogAsync(ProblemMetrics metrics)
    {
        List<ProblemMetrics> allMetrics = new();

        if (File.Exists(_logFile))
        {
            var json = await File.ReadAllTextAsync(_logFile);
            allMetrics = JsonSerializer.Deserialize<List<ProblemMetrics>>(json) ?? new();
        }

        allMetrics.Add(metrics);

        var updatedJson = JsonSerializer.Serialize(allMetrics, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_logFile, updatedJson);
    }
}
