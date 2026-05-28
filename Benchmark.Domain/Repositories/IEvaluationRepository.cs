using Benchmark.Domain.Evaluation;

namespace Benchmark.Domain.Repositories;

public interface IEvaluationRepository
{
    Task AddAsync(EvaluationResult result, CancellationToken ct = default);
    Task<EvaluationResult?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<EvaluationResult>> GetByProblemIdAsync(Guid problemId, CancellationToken ct = default);
    Task<IReadOnlyList<EvaluationResult>> GetWithoutSamplesAsync(int limit, CancellationToken ct = default);
}
