namespace Benchmark.Domain.Training;

public record TrainingResult(
    bool Success,
    ModelVersion? ModelVersion,
    string Message);

public interface ITrainingOrchestrator
{
    Task<TrainingResult> TriggerAsync(CancellationToken ct = default);
}
