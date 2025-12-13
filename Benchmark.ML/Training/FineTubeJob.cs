using Benchmark.ML.LLM;

namespace Benchmark.ML.Training;

public class FineTuneJob
{
    public string DatasetPath { get; set; } = string.Empty;
    public string TargetModel { get; set; } = "base-model";

    private readonly ILlmClient _llmClient;

    public FineTuneJob(ILlmClient llmClient)
    {
        _llmClient = llmClient;
    }

    public async Task<string> RunAsync(CancellationToken cancellationToken = default)
    {
        // Start fine-tuning using the dataset
        string newModelId = await _llmClient.FineTuneAsync(DatasetPath, cancellationToken);
        return newModelId;
    }
}
