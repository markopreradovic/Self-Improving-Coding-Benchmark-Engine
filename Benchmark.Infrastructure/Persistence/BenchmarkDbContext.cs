using Benchmark.Domain.Dataset;
using Benchmark.Domain.Evaluation;
using Benchmark.Domain.Problems;
using Benchmark.Domain.Repositories;
using Benchmark.Domain.Training;
using Microsoft.EntityFrameworkCore;

namespace Benchmark.Infrastructure.Persistence;

public class BenchmarkDbContext : DbContext, IUnitOfWork
{
    public DbSet<CodingProblem> Problems => Set<CodingProblem>();
    public DbSet<EvaluationResult> Evaluations => Set<EvaluationResult>();
    public DbSet<FineTuneSample> FineTuneSamples => Set<FineTuneSample>();
    public DbSet<ModelVersion> ModelVersions => Set<ModelVersion>();

    public BenchmarkDbContext(DbContextOptions<BenchmarkDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BenchmarkDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
