using Benchmark.Engine.Dataset;
using Benchmark.Engine.Evaluation;
using Benchmark.Engine.Problems.Models;
using Benchmark.ML.LLM;

namespace Benchmark.ML.Training;

public class TrainingOrchestrator
{
    private readonly ILlmClient _llmClient;
    private readonly DatasetBuilder _datasetBuilder;
    private readonly CodeEvaluator _evaluator;
    private readonly FailureAnalyzer _failureAnalyzer;

    public TrainingOrchestrator(
        ILlmClient llmClient,
        DatasetBuilder datasetBuilder,
        CodeEvaluator evaluator,
        FailureAnalyzer failureAnalyzer)
    {
        _llmClient = llmClient;
        _datasetBuilder = datasetBuilder;
        _evaluator = evaluator;
        _failureAnalyzer = failureAnalyzer;
    }

    /// <summary>
    /// Runs one closed-loop training iteration
    /// </summary>
    public async Task<string> RunIterationAsync(CodingProblem problem, string solutionCode)
    {
        // 1. Evaluate solution 
        var results = _evaluator.Evaluate(problem, solutionCode);

        // 2. Make dataset from failed cases
        var dataset = _datasetBuilder.BuildDataset(problem, results);
        string datasetPath = $"fine_tune_{Guid.NewGuid()}.jsonl";
        _datasetBuilder.SaveDataset(dataset, datasetPath);

        // 3. Create and run fine-tune job
        var fineTuneJob = new FineTuneJob(_llmClient) { DatasetPath = datasetPath };
        string newModelId = await fineTuneJob.RunAsync();

        return newModelId;
    }
}
