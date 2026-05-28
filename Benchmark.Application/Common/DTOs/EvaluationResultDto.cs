namespace Benchmark.Application.Common.DTOs;

public record TestCaseResultDto(
    string Input,
    string ExpectedOutput,
    string? ActualOutput,
    string Verdict,
    long ExecutionTimeMs,
    string? ErrorMessage);

public record EvaluationResultDto(
    Guid Id,
    Guid ProblemId,
    string ModelName,
    string OverallVerdict,
    int PassedCount,
    int TotalCount,
    double Score,
    string FailureSummary,
    DateTime EvaluatedAt,
    IReadOnlyList<TestCaseResultDto> TestCaseResults);
