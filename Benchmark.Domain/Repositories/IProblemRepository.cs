using Benchmark.Domain.Problems;

namespace Benchmark.Domain.Repositories;

public interface IProblemRepository
{
    Task AddAsync(CodingProblem problem, CancellationToken ct = default);
    Task<CodingProblem?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<(IReadOnlyList<CodingProblem> Items, int TotalCount)> GetPagedAsync(
        ProblemCategory? category,
        DifficultyLevel? difficulty,
        int page,
        int pageSize,
        CancellationToken ct = default);
}
