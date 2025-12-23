using Benchmark.Engine.Problems.Models;
using Benchmark.Engine.Evaluation;
using Benchmark.Engine.Dataset;

namespace Benchmark.Engine.Runner
{
    /// <summary>
    /// Responsible ONLY for running benchmarks:
    /// - solving problems
    /// - evaluating solutions
    /// - collecting metrics
    /// 
    /// NO training, NO ML logic here.
    /// </summary>
    public class BenchmarkRunner
    {
        private readonly CodeEvaluator _evaluator;
        private readonly DatasetBuilder _datasetBuilder;

        public BenchmarkRunner(
            CodeEvaluator evaluator,
            DatasetBuilder datasetBuilder)
        {
            _evaluator = evaluator;
            _datasetBuilder = datasetBuilder;
        }

        /// <summary>
        /// Runs benchmark for a batch of coding problems
        /// </summary>
        /// <param name="problems">List of coding problems</param>
        /// <param name="solverFunc">Function that returns solution code</param>
        public async Task<List<BenchmarkResult>> RunBenchmarksAsync(
            List<CodingProblem> problems,
            Func<CodingProblem, Task<string>> solverFunc)
        {
            var results = new List<BenchmarkResult>();

            foreach (var problem in problems)
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();

                // 1️⃣ Generate solution (LLM / solver)
                string solutionCode = await solverFunc(problem);

                // 2️⃣ Evaluate solution using sandbox
                var evaluationResults = await _evaluator
                    .EvaluateAsync(problem, solutionCode);

                sw.Stop();

                // 3️⃣ Build dataset samples (NO saving here)
                // Dataset is returned to caller (Worker/API)
                var datasetSamples = _datasetBuilder
                    .BuildDataset(problem, evaluationResults);

                // 4️⃣ Collect benchmark metrics
                var benchmarkResult = new BenchmarkResult
                {
                    ProblemTitle = problem.Title,
                    TotalTests = evaluationResults.Count,
                    PassedTests = evaluationResults.Count(r => r.Passed),
                    PassedAllTests = evaluationResults.All(r => r.Passed),
                    ExecutionTime = sw.Elapsed,
                    FailedTestInputs = evaluationResults
                        .Where(r => !r.Passed)
                        .Select(r => r.TestInput)
                        .ToList(),

                    // Optional: expose dataset to upper layer
                    GeneratedDataset = datasetSamples
                };

                results.Add(benchmarkResult);
            }

            return results;
        }
    }
}
