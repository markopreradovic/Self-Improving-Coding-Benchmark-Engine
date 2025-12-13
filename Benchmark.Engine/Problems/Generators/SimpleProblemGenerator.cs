using Benchmark.Engine.Problems.Models;

namespace Benchmark.Engine.Problems.Generators;

public class SimpleProblemGenerator : IProblemGenerator
{
    public Task<CodingProblem> GenerateAsync(
        string category,
        string difficulty,
        CancellationToken cancellationToken = default)
    {
        var problem = new CodingProblem
        {
            Title = "Sum of Two Numbers",
            Description = """
                Given two integers a and b, return their sum.

                Example:
                Input: a = 2, b = 3
                Output: 5
                """,
            Difficulty = difficulty,
            Category = category,
            ExpectedFunctionSignature = "int Sum(int a, int b)",
            TestCases =
            {
                new() { Input = "2,3", ExpectedOutput = "5" },
                new() { Input = "-1,1", ExpectedOutput = "0" },
                new() { Input = "10,20", ExpectedOutput = "30" }
            }
        };

        return Task.FromResult(problem);
    }
}
