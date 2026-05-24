using Benchmark.Domain.Problems;

namespace Benchmark.Application.Generators;

public interface ITypedProblemGenerator : IProblemGenerator
{
    ProblemCategory SupportedCategory { get; }
}
