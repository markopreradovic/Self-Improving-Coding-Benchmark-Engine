namespace Benchmark.ML.Inference;

public record ModelMetadata(
    string ModelName,
    int VersionNumber,
    string FilePath,
    long FileSizeBytes,
    DateTime LoadedAt);
