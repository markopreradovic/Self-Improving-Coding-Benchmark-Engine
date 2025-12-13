using Benchmark.Engine.Problems.Models;
using Benchmark.Engine.Evaluation;

namespace Benchmark.Engine.Dataset;

public class DatasetBuilder
{
    private readonly FailureAnalyzer _failureAnalyzer;

    public DatasetBuilder(FailureAnalyzer failureAnalyzer)
    {
        _failureAnalyzer = failureAnalyzer;
    }

    /// <summary>
    /// Creates a list of FineTuneSample based on failed evaluation results
    /// </summary>
    public List<FineTuneSample> BuildDataset(CodingProblem problem, List<EvaluationResult> results)
    {
        var dataset = new List<FineTuneSample>();

        var failures = _failureAnalyzer.AnalyzeFailures(results);

        foreach (var result in results.Where(r => !r.Passed))
        {
            dataset.Add(new FineTuneSample
            {
                Prompt = $"Problem: {problem.Title}\nDescription: {problem.Description}\nTest Input: {result.TestInput}\nError: {result.ErrorMessage}\n",
                Completion = result.ExpectedOutput,
                Category = problem.Category
            });
        }

        return dataset;
    }

    /// <summary>
    /// Save dataset to a JSONL file at specified file path (OpenAI format)
    /// </summary>
    public void SaveDataset(List<FineTuneSample> dataset, string filePath)
    {
        using var writer = new StreamWriter(filePath);
        foreach (var sample in dataset)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(sample);
            writer.WriteLine(json);
        }
    }
}
