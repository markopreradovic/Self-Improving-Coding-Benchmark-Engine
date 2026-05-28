namespace Benchmark.Worker.Jobs;

public interface IJob
{
    Task RunAsync(CancellationToken ct);
}
