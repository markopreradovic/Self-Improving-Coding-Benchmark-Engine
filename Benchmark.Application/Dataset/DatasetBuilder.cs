using Benchmark.Domain.Dataset;
using Benchmark.Domain.Evaluation;
using Benchmark.Domain.Problems;

namespace Benchmark.Application.Dataset;

public static class DatasetBuilder
{
    public static FineTuneSample? Build(CodingProblem problem, EvaluationResult evaluation)
    {
        if (evaluation.GeneratedCode is null)
            return null;

        return FineTuneSample.Create(
            problem,
            evaluation.Id,
            evaluation.GeneratedCode,
            evaluation.ModelName,
            evaluation.OverallVerdict,
            evaluation.Score);
    }
}
