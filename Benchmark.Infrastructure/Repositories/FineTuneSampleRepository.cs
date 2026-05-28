using Benchmark.Domain.Dataset;
using Benchmark.Domain.Problems;
using Benchmark.Domain.Repositories;
using Benchmark.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Benchmark.Infrastructure.Repositories;

public class FineTuneSampleRepository : IFineTuneSampleRepository
{
    private readonly BenchmarkDbContext _context;

    public FineTuneSampleRepository(BenchmarkDbContext context) => _context = context;

    public async Task AddAsync(FineTuneSample sample, CancellationToken ct = default)
        => await _context.FineTuneSamples.AddAsync(sample, ct);

    public async Task<IReadOnlyList<FineTuneSample>> GetFilteredAsync(
        ProblemCategory? category = null,
        DifficultyLevel? difficulty = null,
        SampleType? sampleType = null,
        double minScore = 0.0,
        CancellationToken ct = default)
    {
        var query = _context.FineTuneSamples.AsQueryable();

        if (category.HasValue)
            query = query.Where(s => s.Category == category.Value);

        if (difficulty.HasValue)
            query = query.Where(s => s.Difficulty == difficulty.Value);

        if (sampleType.HasValue)
            query = query.Where(s => s.SampleType == sampleType.Value);

        if (minScore > 0.0)
            query = query.Where(s => s.Score >= minScore);

        return await query
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(ct);
    }
}
