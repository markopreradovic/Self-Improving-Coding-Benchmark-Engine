using Benchmark.Engine.Problems.Models;

namespace Benchmark.Engine.Solvers;

public class BaseSolver : ISolver
{
    public Task<string> SolveAsync(CodingProblem problem, CancellationToken cancellationToken = default)
    {
        // Just an example, for now solves only our test problem
        string solutionCode = problem.Title switch
        {
            "Sum of Two Numbers" => """
                int Sum(int a, int b) 
                {
                    return a + b;
                }
                """,
            _ => "// TODO: implement solver logic"
        };

        return Task.FromResult(solutionCode);
    }
}
