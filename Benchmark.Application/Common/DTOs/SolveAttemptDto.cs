namespace Benchmark.Application.Common.DTOs;

public record SolveAttemptDto(
    Guid ProblemId,
    string Status,
    string? GeneratedCode,
    string? ErrorMessage,
    double ResponseTimeMs,
    string ModelName);
