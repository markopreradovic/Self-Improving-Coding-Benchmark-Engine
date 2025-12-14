using Benchmark.Engine.Problems.Models;
using Benchmark.Engine.Sandbox;

namespace Benchmark.Engine.Evaluation;

public class CodeEvaluator
{
    private readonly CodeSandbox _sandbox;

    public CodeEvaluator(CodeSandbox sandbox)
    {
        _sandbox = sandbox;
    }

    public async Task<List<EvaluationResult>> EvaluateAsync(
        CodingProblem problem,
        string solutionCode,
        CancellationToken cancellationToken = default)
    {
        var results = new List<EvaluationResult>();

        foreach (var test in problem.TestCases)
        {
            var singleTestProblem = new CodingProblem
            {
                Title = problem.Title,
                Description = problem.Description,
                ExpectedFunctionSignature = problem.ExpectedFunctionSignature,
                TestCases = new() { test }
            };

            var sandboxResult = await _sandbox.ExecuteAsync(
                singleTestProblem,
                solutionCode,
                cancellationToken);

            results.Add(new EvaluationResult
            {
                TestInput = test.Input,
                ExpectedOutput = test.ExpectedOutput,
                ActualOutput = sandboxResult.Output,
                Passed = sandboxResult.Success &&
                         sandboxResult.Output == test.ExpectedOutput,
                ErrorMessage = sandboxResult.Error
            });
        }

        return results;
    }
}
