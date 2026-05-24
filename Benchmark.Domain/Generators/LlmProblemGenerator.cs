using Benchmark.Domain.Problems;
using ErrorOr;

namespace Benchmark.Application.Generators;

// Skeleton for Phase 7 – requires ILlmClient from Benchmark.ML to be wired up.
public class LlmProblemGenerator : IProblemGenerator
{
    public ErrorOr<CodingProblem> Generate(
        ProblemCategory category,
        DifficultyLevel? difficulty = null,
        int minTestCases = 3)
    {
        return Error.Failure(
            "LlmGenerator.NotConfigured",
            "LLM problem generator is not yet configured. Wire up ILlmClient in Phase 7.");
    }
}
