namespace Benchmark.Domain.Evaluation;

public class EvaluationResult
{
    public Guid Id { get; private set; }
    public Guid ProblemId { get; private set; }
    public string ModelName { get; private set; } = string.Empty;
    public EvaluationVerdict OverallVerdict { get; private set; }
    public int PassedCount { get; private set; }
    public int TotalCount { get; private set; }
    public double Score { get; private set; }
    public string? GeneratedCode { get; private set; }
    public DateTime EvaluatedAt { get; private set; }

    public IReadOnlyList<TestCaseResult> TestCaseResults => _testCaseResults.AsReadOnly();
    private readonly List<TestCaseResult> _testCaseResults = new();

    private EvaluationResult() { }

    public static EvaluationResult Create(
        Guid problemId,
        string modelName,
        string? generatedCode,
        IEnumerable<TestCaseResult> results)
    {
        var list = results.ToList();
        int passed = list.Count(r => r.Verdict == EvaluationVerdict.Accepted);

        var evaluation = new EvaluationResult
        {
            Id = Guid.NewGuid(),
            ProblemId = problemId,
            ModelName = modelName,
            GeneratedCode = generatedCode,
            OverallVerdict = DetermineOverallVerdict(list),
            PassedCount = passed,
            TotalCount = list.Count,
            Score = list.Count > 0 ? (double)passed / list.Count : 0.0,
            EvaluatedAt = DateTime.UtcNow
        };

        foreach (var r in list)
            evaluation._testCaseResults.Add(r);

        return evaluation;
    }

    private static EvaluationVerdict DetermineOverallVerdict(List<TestCaseResult> results)
    {
        if (results.Any(r => r.Verdict == EvaluationVerdict.CompileError))
            return EvaluationVerdict.CompileError;
        if (results.All(r => r.Verdict == EvaluationVerdict.Accepted))
            return EvaluationVerdict.Accepted;
        if (results.Any(r => r.Verdict == EvaluationVerdict.TimeLimitExceeded))
            return EvaluationVerdict.TimeLimitExceeded;
        if (results.Any(r => r.Verdict == EvaluationVerdict.RuntimeError))
            return EvaluationVerdict.RuntimeError;
        return EvaluationVerdict.WrongAnswer;
    }
}
