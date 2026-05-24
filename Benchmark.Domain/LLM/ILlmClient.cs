namespace Benchmark.Domain.LLM;

public interface ILlmClient
{
    string ModelName { get; }

    Task<string> CompleteAsync(
        string prompt,
        int maxTokens = 4096,
        CancellationToken ct = default);
}
