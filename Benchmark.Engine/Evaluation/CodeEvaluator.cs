using Benchmark.Engine.Problems.Models;

namespace Benchmark.Engine.Evaluation;

public class CodeEvaluator
{
    /// <summary>
    /// Evaluate solution code against problem test cases.
    /// For now only simulates environment for our test case.
    /// </summary>
    public List<EvaluationResult> Evaluate(CodingProblem problem, string solutionCode)
    {
        var results = new List<EvaluationResult>();

        foreach (var test in problem.TestCases)
        {
            // Za sada stub: ako problem title == "Sum of Two Numbers" simuliramo pass/fail
            bool passed = problem.Title == "Sum of Two Numbers" && test.ExpectedOutput == "5";

            results.Add(new EvaluationResult
            {
                TestInput = test.Input,
                ExpectedOutput = test.ExpectedOutput,
                ActualOutput = passed ? test.ExpectedOutput : "0", // simulirani rezultat
                Passed = passed,
                ErrorMessage = passed ? string.Empty : "Stub evaluator failed"
            });
        }

        return results;
    }
}
