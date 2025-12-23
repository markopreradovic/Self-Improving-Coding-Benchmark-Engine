namespace Benchmark.ML.LLM;

public interface ILlmClient
{
    
    Task<string> GenerateCodeAsync(string prompt);

    
    Task<string> FineTuneAsync(string datasetPath, CancellationToken cancellationToken = default);
}
