using Benchmark.Domain.Problems;
using Benchmark.Domain.Repositories;
using Benchmark.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Benchmark.Infrastructure.Repositories;

public class ProblemRepository : IProblemRepository
{
    private readonly BenchmarkDbContext _context;

    public ProblemRepository(BenchmarkDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CodingProblem problem, CancellationToken ct = default)
    {
        await _context.Problems.AddAsync(problem, ct);
    }

    public async Task<CodingProblem?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        // Owned entities (TestCases) are auto-loaded in tracking queries
        return await _context.Problems
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<(IReadOnlyList<CodingProblem> Items, int TotalCount)> GetPagedAsync(
        ProblemCategory? category,
        DifficultyLevel? difficulty,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var query = _context.Problems.AsNoTracking().AsQueryable();

        if (category.HasValue)
            query = query.Where(p => p.Category == category.Value);

        if (difficulty.HasValue)
            query = query.Where(p => p.Difficulty == difficulty.Value);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}
