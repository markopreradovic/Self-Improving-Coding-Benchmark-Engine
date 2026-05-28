using Benchmark.Domain.Evaluation;
using Benchmark.Domain.Problems;
using Benchmark.Domain.Sandbox;

namespace Benchmark.Application.Evaluation;

public class CodeEvaluator : IEvaluator
{
    private readonly ICodeRunner _runner;

    public CodeEvaluator(ICodeRunner runner) => _runner = runner;

    public async Task<EvaluationResult> EvaluateAsync(
        CodingProblem problem,
        string code,
        string modelName,
        CancellationToken ct = default)
    {
        var results = new List<TestCaseResult>();

        foreach (var testCase in problem.TestCases)
        {
            var execution = await _runner.RunAsync(code, testCase.Input, ct: ct);
            var verdict = MapVerdict(execution.Status, execution.Output, testCase.ExpectedOutput);

            results.Add(TestCaseResult.Create(
                testCase.Input,
                testCase.ExpectedOutput,
                execution.Output,
                verdict,
                execution.ExecutionTime,
                execution.ErrorMessage));

            // All remaining test cases will fail the same way on a compile error
            if (verdict == EvaluationVerdict.CompileError)
            {
                foreach (var remaining in problem.TestCases.Skip(results.Count))
                    results.Add(TestCaseResult.Create(
                        remaining.Input, remaining.ExpectedOutput,
                        null, EvaluationVerdict.CompileError, TimeSpan.Zero,
                        "Skipped due to compile error."));
                break;
            }
        }

        return EvaluationResult.Create(problem.Id, modelName, code, results);
    }

    private static EvaluationVerdict MapVerdict(ExecutionStatus status, string? actual, string expected)
        => status switch
        {
            ExecutionStatus.Accepted         => NormalizeOutput(actual) == NormalizeOutput(expected)
                                                    ? EvaluationVerdict.Accepted
                                                    : EvaluationVerdict.WrongAnswer,
            ExecutionStatus.TimeLimitExceeded => EvaluationVerdict.TimeLimitExceeded,
            ExecutionStatus.RuntimeError      => EvaluationVerdict.RuntimeError,
            ExecutionStatus.CompileError      => EvaluationVerdict.CompileError,
            _                                 => EvaluationVerdict.WrongAnswer
        };

    private static string NormalizeOutput(string? output)
        => (output ?? string.Empty).Trim().ReplaceLineEndings("\n");
}
