using Benchmark.Domain.Problems;

namespace Benchmark.Domain.Evaluation;

public interface IEvaluator
{
    Task<EvaluationResult> EvaluateAsync(
        CodingProblem problem,
        string code,
        string modelName,
        CancellationToken ct = default);
}
