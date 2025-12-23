using Benchmark.Engine.Dataset;
using Benchmark.Engine.Problems.Models;

namespace Benchmark.Engine.Runner
{
    public class BenchmarkResult
    {
        public string ProblemTitle { get; set; } = string.Empty;

        public CodingProblem Problem { get; set; }
        public bool PassedAllTests { get; set; }
        public int TotalTests { get; set; }
        public int PassedTests { get; set; }

        public double Accuracy =>
            TotalTests == 0 ? 0 : (double)PassedTests / TotalTests;

        public TimeSpan ExecutionTime { get; set; }

        public List<string> FailedTestInputs { get; set; } = new();

        // 🔥 Dataset samples generated from failures
        public List<FineTuneSample> GeneratedDataset { get; set; } = new();
    }
}
