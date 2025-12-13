namespace Benchmark.ML.LLM;

public interface ILlmClient
{
    /// <summary>
    /// Generates code solution for a given prompt
    /// </summary>
    Task<string> GenerateCodeAsync(string prompt, CancellationToken cancellationToken = default);

    /// <summary>
    /// Fine-tunes a model with provided JSONL dataset
    /// </summary>
    Task<string> FineTuneAsync(string datasetFilePath, CancellationToken cancellationToken = default);
}
