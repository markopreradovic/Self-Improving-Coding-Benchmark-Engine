using Benchmark.Domain.Repositories;
using Benchmark.Infrastructure.Persistence;
using Benchmark.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Benchmark.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Data Source=benchmark.db";

        // Switch to UseNpgsql(...) for PostgreSQL in production
        services.AddDbContext<BenchmarkDbContext>(opts =>
            opts.UseSqlite(connectionString,
                b => b.MigrationsAssembly(typeof(BenchmarkDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<BenchmarkDbContext>());

        services.AddScoped<IProblemRepository, ProblemRepository>();

        return services;
    }

    public static async Task ApplyMigrationsAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BenchmarkDbContext>();
        await db.Database.MigrateAsync();
    }
}
