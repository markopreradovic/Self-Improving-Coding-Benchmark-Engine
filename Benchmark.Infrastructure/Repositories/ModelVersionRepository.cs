using Benchmark.Domain.Repositories;
using Benchmark.Domain.Training;
using Benchmark.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Benchmark.Infrastructure.Repositories;

public class ModelVersionRepository : IModelVersionRepository
{
    private readonly BenchmarkDbContext _context;

    public ModelVersionRepository(BenchmarkDbContext context) => _context = context;

    public async Task AddAsync(ModelVersion version, CancellationToken ct = default)
        => await _context.ModelVersions.AddAsync(version, ct);

    public async Task<IReadOnlyList<ModelVersion>> GetAllAsync(CancellationToken ct = default)
        => await _context.ModelVersions
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync(ct);

    public async Task<ModelVersion?> GetLatestActiveAsync(CancellationToken ct = default)
        => await _context.ModelVersions
            .Where(v => v.IsActive)
            .OrderByDescending(v => v.VersionNumber)
            .FirstOrDefaultAsync(ct);

    public async Task<int> GetNextVersionNumberAsync(string modelName, CancellationToken ct = default)
    {
        var max = await _context.ModelVersions
            .Where(v => v.ModelName == modelName)
            .MaxAsync(v => (int?)v.VersionNumber, ct);
        return (max ?? 0) + 1;
    }
}
