using Benchmark.Domain.Metrics;
using Benchmark.Domain.Repositories;
using Benchmark.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Benchmark.Infrastructure.Repositories;

public class MetricsRepository : IMetricsRepository
{
    private readonly BenchmarkDbContext _context;

    public MetricsRepository(BenchmarkDbContext context) => _context = context;

    public Task<int> GetTotalProblemsAsync(CancellationToken ct = default)
        => _context.Problems.CountAsync(ct);

    public Task<int> GetTotalEvaluationsAsync(CancellationToken ct = default)
        => _context.Evaluations.CountAsync(ct);

    public Task<int> GetTotalSamplesAsync(CancellationToken ct = default)
        => _context.FineTuneSamples.CountAsync(ct);

    public async Task<double> GetOverallSuccessRateAsync(CancellationToken ct = default)
    {
        if (!await _context.Evaluations.AnyAsync(ct)) return 0;
        return await _context.Evaluations.AverageAsync(e => e.Score, ct);
    }

    public async Task<IReadOnlyList<HeatmapEntry>> GetHeatmapDataAsync(CancellationToken ct = default)
    {
        var data = await (
            from e in _context.Evaluations
            join p in _context.Problems on e.ProblemId equals p.Id
            group e by new { p.Category, p.Difficulty } into g
            select new
            {
                Category   = g.Key.Category.ToString(),
                Difficulty = g.Key.Difficulty.ToString(),
                Total      = g.Count(),
                AvgScore   = g.Average(x => x.Score)
            }
        ).ToListAsync(ct);

        return data.Select(d => new HeatmapEntry(d.Category, d.Difficulty, d.Total, d.AvgScore))
                   .ToList();
    }
}
