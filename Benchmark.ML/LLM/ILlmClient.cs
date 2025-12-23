namespace Benchmark.ML.LLM;

public interface ILlmClient
{
    /// <summary>
    /// Generates code from a text prompt using an LLM
    /// </summary>
    Task<string> GenerateCodeAsync(string prompt);

    /// <summary>
    /// Fine-tune the model using a dataset
    /// </summary>
    /// <param name="datasetPath">Path to dataset file</param>
    /// <param name="cancellationToken"></param>
    /// <returns>New model ID</returns>
    Task<string> FineTuneAsync(string datasetPath, CancellationToken cancellationToken = default);
}
