using Benchmark.Engine.Problems.Models;
using Benchmark.Engine.Evaluation;
using Benchmark.Engine.Dataset;

namespace Benchmark.Engine.Runner
{
    
    public class BenchmarkRunner
    {
        private readonly DatasetBuilder _datasetBuilder;
        private readonly CodeEvaluator _codeEvaluator;

        public BenchmarkRunner(DatasetBuilder datasetBuilder, CodeEvaluator codeEvaluator)
        {
            _datasetBuilder = datasetBuilder;
            _codeEvaluator = codeEvaluator;
        }

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
                var evaluationResults = await _codeEvaluator
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
