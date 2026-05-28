namespace Benchmark.Domain.Metrics;

public record HeatmapEntry(
    string Category,
    string Difficulty,
    int Total,
    double AvgScore);
