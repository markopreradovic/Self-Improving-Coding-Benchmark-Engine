using Benchmark.Domain.Problems;

namespace Benchmark.Domain.LLM;

public interface ISolver
{
    Task<SolveAttempt> SolveAsync(CodingProblem problem, CancellationToken ct = default);
}
