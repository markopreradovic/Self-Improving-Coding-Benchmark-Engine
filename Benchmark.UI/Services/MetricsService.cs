using Benchmark.Engine.Metrics;
using System.Text.Json;

namespace Benchmark.UI.Services
{
    public class MetricsService
    {
        private readonly string _metricsFile = "metrics_log.json";

        public async Task<List<ProblemMetrics>> GetMetricsAsync()
        {
            if (!File.Exists(_metricsFile))
                return new List<ProblemMetrics>();

            var json = await File.ReadAllTextAsync(_metricsFile);
            return JsonSerializer.Deserialize<List<ProblemMetrics>>(json) ?? new List<ProblemMetrics>();
        }
    }
}
