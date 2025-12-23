using Benchmark.Engine.Problems.Models;
using Benchmark.Engine.Runner;
using Benchmark.ML.Training;
using Microsoft.Extensions.Hosting;

namespace Benchmark.Worker;

public class Worker : BackgroundService
{
    private readonly BenchmarkRunner _benchmarkRunner;
    private readonly TrainingOrchestrator _trainingOrchestrator;

    public Worker(
        BenchmarkRunner benchmarkRunner,
        TrainingOrchestrator trainingOrchestrator)
    {
        _benchmarkRunner = benchmarkRunner;
        _trainingOrchestrator = trainingOrchestrator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // 1️⃣ Dummy problem (za test)
        var problem = new CodingProblem
        {
            Title = "Sum Two Numbers",
            ExpectedFunctionSignature = "int Sum(int a, int b)",
            TestCases =
            {
                new() { Input = "2,3", ExpectedOutput = "5" },
                new() { Input = "5,7", ExpectedOutput = "12" }
            }
        };

        // 2️⃣ Dummy solver (kasnije LLM)
        async Task<string> Solver(CodingProblem p)
        {
            return """
            int Sum(int a, int b)
            {
                return a + b;
            }
            """;
        }

        // 3️⃣ Run benchmark
        var results = await _benchmarkRunner.RunBenchmarksAsync(
            new List<CodingProblem> { problem },
            Solver);

        // 4️⃣ Training loop (self-improving)
        foreach (var result in results)
        {
            if (!result.PassedAllTests)
            {
                await _trainingOrchestrator.RunIterationAsync(
                    problem,
                    await Solver(problem));
            }
        }
    }
}
