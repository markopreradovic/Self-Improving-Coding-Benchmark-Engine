using Benchmark.Engine.Metrics;
using Benchmark.Engine.Problems;
using Benchmark.Engine.Problems.Models;
using Benchmark.Engine.Runner;
using Benchmark.ML.LLM;
using Benchmark.ML.Training;
using Microsoft.Extensions.Hosting;

namespace Benchmark.Worker;

public class Worker : BackgroundService
{
    private readonly BenchmarkRunner _benchmarkRunner;
    private readonly TrainingOrchestrator _trainingOrchestrator;
    private readonly ILlmClient _llmClient;

    public Worker(
        BenchmarkRunner benchmarkRunner,
        TrainingOrchestrator trainingOrchestrator,
        ILlmClient llmClient)
    {
        _benchmarkRunner = benchmarkRunner;
        _trainingOrchestrator = trainingOrchestrator;
        _llmClient = llmClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var generator = new ProblemGenerator();

        var categories = new[] { "Arrays", "DP", "Graphs", "Strings", "Trees", "Backtracking", "Sorting" };
        var difficulties = new[] { "Easy", "Medium" };

        foreach (var category in categories)
        {
            foreach (var difficulty in difficulties)
            {
                var problems = await generator.GenerateBatchAsync(category, difficulty, count: 5);

                async Task<string> Solver(CodingProblem problem)
                {
                    string prompt = problem.ToPrompt();
                    return await _llmClient.GenerateCodeAsync(prompt);
                }

                var benchmarkResults = await _benchmarkRunner.RunBenchmarksAsync(problems, Solver);

                // ← OVDJE ide tvoj blok za metrics logging i training loop
                for (int i = 0; i < benchmarkResults.Count; i++)
                {
                    var result = benchmarkResults[i];
                    var problem = problems[i];

                    int trainingIterations = 0;
                    if (!result.PassedAllTests)
                    {
                        var solutionCode = await Solver(problem);
                        await _trainingOrchestrator.RunIterationAsync(problem, solutionCode);
                        trainingIterations = 1;
                    }

                    var metrics = new ProblemMetrics
                    {
                        Title = problem.Title,
                        Category = problem.Category,
                        Difficulty = problem.Difficulty,
                        PassedTests = result.PassedTests,
                        TotalTests = result.TotalTests,
                        Accuracy = result.Accuracy,
                        TrainingIterations = trainingIterations
                    };

                    await MetricsLogger.LogAsync(metrics);
                }
            }
        }
    }

}
