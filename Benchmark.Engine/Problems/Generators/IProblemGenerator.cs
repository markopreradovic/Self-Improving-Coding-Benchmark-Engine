using Benchmark.Engine.Problems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Engine.Problems.Generators
{
    public interface IProblemGenerator
    {
        Task<CodingProblem> GenerateAsync(string category, string difficulty, CancellationToken cancellationToken = default);
    }
}
