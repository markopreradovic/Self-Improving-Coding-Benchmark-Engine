namespace Benchmark.Domain.Training;

public class ModelVersion
{
    public Guid Id { get; private set; }
    public string ModelName { get; private set; } = string.Empty;
    public int VersionNumber { get; private set; }
    public string BaseModel { get; private set; } = string.Empty;
    public string? FilePath { get; private set; }
    public double AccuracyScore { get; private set; }
    public int TrainingSamples { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private ModelVersion() { }

    public static ModelVersion Create(
        string modelName,
        int versionNumber,
        string baseModel,
        string? filePath,
        double accuracyScore,
        int trainingSamples)
        => new()
        {
            Id              = Guid.NewGuid(),
            ModelName       = modelName,
            VersionNumber   = versionNumber,
            BaseModel       = baseModel,
            FilePath        = filePath,
            AccuracyScore   = accuracyScore,
            TrainingSamples = trainingSamples,
            IsActive        = true,
            CreatedAt       = DateTime.UtcNow
        };

    public void Deactivate() => IsActive = false;
}
