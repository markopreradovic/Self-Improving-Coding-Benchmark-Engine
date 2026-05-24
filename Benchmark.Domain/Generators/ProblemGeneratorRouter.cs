using Benchmark.Domain.Problems;
using Benchmark.Domain.Problems.Common;
using ErrorOr;

namespace Benchmark.Application.Generators;

public class ProblemGeneratorRouter : IProblemGenerator
{
    private readonly IReadOnlyDictionary<ProblemCategory, ITypedProblemGenerator> _generators;

    public ProblemGeneratorRouter(IEnumerable<ITypedProblemGenerator> generators)
    {
        _generators = generators.ToDictionary(g => g.SupportedCategory);
    }

    public ErrorOr<CodingProblem> Generate(
        ProblemCategory category,
        DifficultyLevel? difficulty = null,
        int minTestCases = 3)
    {
        if (!_generators.TryGetValue(category, out var generator))
            return DomainErrors.Problem.UnsupportedCategory;

        return generator.Generate(category, difficulty, minTestCases);
    }
}
