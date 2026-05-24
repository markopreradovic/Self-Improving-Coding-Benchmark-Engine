namespace Benchmark.Domain.Sandbox;

public record ExecutionResult(
    ExecutionStatus Status,
    string? Output,
    string? ErrorMessage,
    TimeSpan ExecutionTime);
