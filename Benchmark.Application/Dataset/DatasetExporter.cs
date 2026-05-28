using System.Text;
using System.Text.Json;
using Benchmark.Domain.Dataset;

namespace Benchmark.Application.Dataset;

public static class DatasetExporter
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static string ExportToJsonl(IEnumerable<FineTuneSample> samples, ExportFormat format)
    {
        var sb = new StringBuilder();

        foreach (var sample in samples)
        {
            var line = format switch
            {
                ExportFormat.OpenAI      => SerializeOpenAi(sample),
                ExportFormat.HuggingFace => SerializeHuggingFace(sample),
                _                        => throw new ArgumentOutOfRangeException(nameof(format))
            };
            sb.AppendLine(line);
        }

        return sb.ToString();
    }

    private static string SerializeOpenAi(FineTuneSample sample)
    {
        var obj = new
        {
            messages = new[]
            {
                new { role = "system",    content = "You are an expert C# programmer solving LeetCode-style coding problems." },
                new { role = "user",      content = BuildUserPrompt(sample) },
                new { role = "assistant", content = $"```csharp\n{sample.Code}\n```" }
            }
        };
        return JsonSerializer.Serialize(obj, _jsonOptions);
    }

    private static string SerializeHuggingFace(FineTuneSample sample)
    {
        var obj = new
        {
            instruction = $"Solve the following {sample.Difficulty} {sample.Category} coding problem in C#.",
            input       = BuildUserPrompt(sample),
            output      = $"```csharp\n{sample.Code}\n```",
            metadata    = new
            {
                category   = sample.Category.ToString(),
                difficulty = sample.Difficulty.ToString(),
                verdict    = sample.Verdict.ToString(),
                score      = sample.Score,
                model      = sample.ModelName,
                sampleType = sample.SampleType.ToString()
            }
        };
        return JsonSerializer.Serialize(obj, _jsonOptions);
    }

    private static string BuildUserPrompt(FineTuneSample sample)
        => $"""
            ## Problem: {sample.ProblemTitle}

            {sample.ProblemDescription}

            ## Function Signature
            {sample.FunctionSignature}
            """;
}
