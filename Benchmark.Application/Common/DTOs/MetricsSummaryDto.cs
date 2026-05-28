namespace Benchmark.Application.Common.DTOs;

public record HeatmapEntryDto(
    string Category,
    string Difficulty,
    int Total,
    double AvgScore);

public record MetricsSummaryDto(
    int TotalProblems,
    int TotalEvaluations,
    int TotalSamples,
    double OverallSuccessRate,
    IReadOnlyList<HeatmapEntryDto> Heatmap);
