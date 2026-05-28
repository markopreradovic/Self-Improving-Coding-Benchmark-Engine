using Benchmark.Domain.Metrics;

namespace Benchmark.Domain.Repositories;

public interface IMetricsRepository
{
    Task<int> GetTotalProblemsAsync(CancellationToken ct = default);
    Task<int> GetTotalEvaluationsAsync(CancellationToken ct = default);
    Task<int> GetTotalSamplesAsync(CancellationToken ct = default);
    Task<double> GetOverallSuccessRateAsync(CancellationToken ct = default);
    Task<IReadOnlyList<HeatmapEntry>> GetHeatmapDataAsync(CancellationToken ct = default);
}
