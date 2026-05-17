using Benchmark.Domain.Problems;
using Benchmark.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Benchmark.Infrastructure.Persistence;

public class BenchmarkDbContext : DbContext, IUnitOfWork
{
    public DbSet<CodingProblem> Problems => Set<CodingProblem>();

    public BenchmarkDbContext(DbContextOptions<BenchmarkDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BenchmarkDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
