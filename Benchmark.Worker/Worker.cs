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

        // 🔹 Batch loop – kategorije i težine
        var categories = new[] { "Arrays", "DP", "Graphs" };
        var difficulties = new[] { "Easy", "Medium" };

        foreach (var category in categories)
        {
            foreach (var difficulty in difficulties)
            {
                Console.WriteLine($"Generating problems: {category} / {difficulty}");

                var problems = await generator.GenerateBatchAsync(category, difficulty, count: 5);

                // 🔹 Solver funkcija preko LLM
                async Task<string> Solver(CodingProblem problem)
                {
                    string prompt = problem.ToPrompt();
                    return await _llmClient.GenerateCodeAsync(prompt);
                }

                // 🔹 Benchmark i evaluacija
                var benchmarkResults = await _benchmarkRunner.RunBenchmarksAsync(
                    problems,
                    Solver
                );

                foreach (var result in benchmarkResults)
                {
                    Console.WriteLine($"{result.ProblemTitle}: {result.PassedTests}/{result.TotalTests} passed, Accuracy={result.Accuracy:P}");

                    // 🔹 Self-improving loop
                    if (!result.PassedAllTests)
                    {
                        var solutionCode = await Solver(problems.First()); // konkretni problem
                        await _trainingOrchestrator.RunIterationAsync(
                            problems.First(),
                            solutionCode
                        );

                        Console.WriteLine($"Training iteration completed for {problems.First().Title}");
                    }
                }
            }
        }

        Console.WriteLine("All batches completed!");
    }
}
