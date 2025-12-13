using Benchmark.Engine.Problems.Models;

namespace Benchmark.Engine.Solvers;

public interface ISolver
{
    /// <summary>
    /// Solve the given coding problem and return code as string.
    /// </summary>
    /// <param name="problem">Coding problem to solve</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Generated solution code</returns>
    Task<string> SolveAsync(CodingProblem problem, CancellationToken cancellationToken = default);
}
