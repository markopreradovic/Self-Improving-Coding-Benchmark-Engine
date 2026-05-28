using Benchmark.Domain.Dataset;
using Benchmark.Domain.Problems;

namespace Benchmark.Domain.Repositories;

public interface IFineTuneSampleRepository
{
    Task AddAsync(FineTuneSample sample, CancellationToken ct = default);
    Task<IReadOnlyList<FineTuneSample>> GetFilteredAsync(
        ProblemCategory? category = null,
        DifficultyLevel? difficulty = null,
        SampleType? sampleType = null,
        double minScore = 0.0,
        CancellationToken ct = default);
}
