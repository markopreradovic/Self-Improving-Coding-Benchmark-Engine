using Benchmark.Domain.Evaluation;
using Benchmark.Domain.Repositories;
using Benchmark.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Benchmark.Infrastructure.Repositories;

public class EvaluationRepository : IEvaluationRepository
{
    private readonly BenchmarkDbContext _context;

    public EvaluationRepository(BenchmarkDbContext context) => _context = context;

    public async Task AddAsync(EvaluationResult result, CancellationToken ct = default)
        => await _context.Evaluations.AddAsync(result, ct);

    public async Task<EvaluationResult?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Evaluations
            .FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<IReadOnlyList<EvaluationResult>> GetByProblemIdAsync(
        Guid problemId, CancellationToken ct = default)
        => await _context.Evaluations
            .Where(e => e.ProblemId == problemId)
            .OrderByDescending(e => e.EvaluatedAt)
            .ToListAsync(ct);
}
