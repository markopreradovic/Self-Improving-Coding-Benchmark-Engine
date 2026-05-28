namespace Benchmark.Application.Common.DTOs;

public record FineTuneSampleDto(
    Guid Id,
    Guid ProblemId,
    Guid EvaluationId,
    string ProblemTitle,
    string Category,
    string Difficulty,
    string ModelName,
    string SampleType,
    string Verdict,
    double Score,
    DateTime CreatedAt);
