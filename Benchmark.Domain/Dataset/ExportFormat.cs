namespace Benchmark.Domain.Dataset;

public enum ExportFormat
{
    OpenAI,       // chat-completions fine-tuning JSONL
    HuggingFace   // instruction/input/output JSONL
}
