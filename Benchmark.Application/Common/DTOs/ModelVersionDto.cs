namespace Benchmark.Application.Common.DTOs;

public record ModelVersionDto(
    Guid Id,
    string ModelName,
    int VersionNumber,
    string BaseModel,
    string? FilePath,
    double AccuracyScore,
    int TrainingSamples,
    bool IsActive,
    DateTime CreatedAt);
