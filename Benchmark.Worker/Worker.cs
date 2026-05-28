using Benchmark.Worker.Jobs;
using Microsoft.Extensions.Options;

namespace Benchmark.Worker;

public class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly WorkerOptions _options;
    private readonly ILogger<Worker> _logger;

    public Worker(
        IServiceScopeFactory scopeFactory,
        IOptions<WorkerOptions> options,
        ILogger<Worker> logger)
    {
        _scopeFactory = scopeFactory;
        _options      = options.Value;
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Benchmark Worker started. Cycle interval: {Interval}s",
            _options.JobIntervalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            await RunCycleAsync(stoppingToken);

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(_options.JobIntervalSeconds), stoppingToken);
            }
            catch (OperationCanceledException) { /* shutting down */ }
        }
    }

    private async Task RunCycleAsync(CancellationToken ct)
    {
        _logger.LogInformation("Benchmark cycle starting at {Time}", DateTimeOffset.UtcNow);

        using var scope = _scopeFactory.CreateScope();
        var sp = scope.ServiceProvider;

        await SafeRunAsync(sp.GetRequiredService<ProblemGenerationJob>(), ct);
        await SafeRunAsync(sp.GetRequiredService<EvaluationJob>(), ct);
        await SafeRunAsync(sp.GetRequiredService<TrainingJob>(), ct);

        _logger.LogInformation("Benchmark cycle completed at {Time}", DateTimeOffset.UtcNow);
    }

    private async Task SafeRunAsync(IJob job, CancellationToken ct)
    {
        try
        {
            await job.RunAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Job} threw an unhandled exception", job.GetType().Name);
        }
    }
}
