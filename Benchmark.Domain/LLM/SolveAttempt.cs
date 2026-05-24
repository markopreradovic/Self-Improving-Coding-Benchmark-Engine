namespace Benchmark.Domain.LLM;

public record SolveAttempt(
    Guid ProblemId,
    SolveStatus Status,
    string? GeneratedCode,
    string? ErrorMessage,
    TimeSpan ResponseTime,
    string ModelName);
