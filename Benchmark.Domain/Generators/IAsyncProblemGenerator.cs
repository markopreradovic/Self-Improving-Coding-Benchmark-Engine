using Benchmark.Domain.Problems;
using ErrorOr;

namespace Benchmark.Application.Generators;

public interface IAsyncProblemGenerator
{
    Task<ErrorOr<CodingProblem>> GenerateAsync(
        ProblemCategory category,
        DifficultyLevel? difficulty = null,
        int minTestCases = 3,
        CancellationToken ct = default);
}
