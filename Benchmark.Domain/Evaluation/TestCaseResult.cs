namespace Benchmark.Domain.Evaluation;

public class TestCaseResult
{
    public string Input { get; private set; } = string.Empty;
    public string ExpectedOutput { get; private set; } = string.Empty;
    public string? ActualOutput { get; private set; }
    public EvaluationVerdict Verdict { get; private set; }
    public long ExecutionTimeMs { get; private set; }
    public string? ErrorMessage { get; private set; }

    private TestCaseResult() { }

    public static TestCaseResult Create(
        string input,
        string expectedOutput,
        string? actualOutput,
        EvaluationVerdict verdict,
        TimeSpan executionTime,
        string? errorMessage = null)
        => new()
        {
            Input = input,
            ExpectedOutput = expectedOutput,
            ActualOutput = actualOutput,
            Verdict = verdict,
            ExecutionTimeMs = (long)executionTime.TotalMilliseconds,
            ErrorMessage = errorMessage
        };
}
