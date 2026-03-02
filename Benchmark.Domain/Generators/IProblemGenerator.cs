using Benchmark.Domain.Problems;
using ErrorOr;

namespace Benchmark.Application.Generators;

public interface IProblemGenerator
{
    ErrorOr<CodingProblem> Generate(
        ProblemCategory category,
        DifficultyLevel? difficulty = null,
        int minTestCases = 3);
}