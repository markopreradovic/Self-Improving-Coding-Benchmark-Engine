namespace Benchmark.Domain.Evaluation;

public record FailureAnalysis(
    int TotalTests,
    int Passed,
    int WrongAnswer,
    int TimeLimitExceeded,
    int RuntimeError,
    int CompileError,
    string Summary);

public static class FailureAnalyzer
{
    public static FailureAnalysis Analyze(EvaluationResult result)
    {
        var wa  = result.TestCaseResults.Count(r => r.Verdict == EvaluationVerdict.WrongAnswer);
        var tle = result.TestCaseResults.Count(r => r.Verdict == EvaluationVerdict.TimeLimitExceeded);
        var re  = result.TestCaseResults.Count(r => r.Verdict == EvaluationVerdict.RuntimeError);
        var ce  = result.TestCaseResults.Count(r => r.Verdict == EvaluationVerdict.CompileError);

        var summary = result.OverallVerdict switch
        {
            EvaluationVerdict.Accepted          => $"All {result.TotalCount} test cases passed.",
            EvaluationVerdict.CompileError       => "Code failed to compile.",
            EvaluationVerdict.TimeLimitExceeded  => $"TLE on {tle} test case(s). Passed {result.PassedCount}/{result.TotalCount}.",
            EvaluationVerdict.RuntimeError       => $"Runtime error on {re} test case(s). Passed {result.PassedCount}/{result.TotalCount}.",
            EvaluationVerdict.WrongAnswer        => $"Wrong answer on {wa} test case(s). Passed {result.PassedCount}/{result.TotalCount}.",
            _                                    => $"Passed {result.PassedCount}/{result.TotalCount}."
        };

        return new FailureAnalysis(result.TotalCount, result.PassedCount, wa, tle, re, ce, summary);
    }
}
