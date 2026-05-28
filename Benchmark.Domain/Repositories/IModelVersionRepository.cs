using Benchmark.Domain.Training;

namespace Benchmark.Domain.Repositories;

public interface IModelVersionRepository
{
    Task AddAsync(ModelVersion version, CancellationToken ct = default);
    Task<IReadOnlyList<ModelVersion>> GetAllAsync(CancellationToken ct = default);
    Task<ModelVersion?> GetLatestActiveAsync(CancellationToken ct = default);
    Task<int> GetNextVersionNumberAsync(string modelName, CancellationToken ct = default);
}
