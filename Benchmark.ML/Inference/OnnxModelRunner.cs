using Microsoft.Extensions.Logging;

namespace Benchmark.ML.Inference;

/// <summary>
/// Local ONNX inference runner. Requires a fine-tuned .onnx model file.
/// Add Microsoft.ML.OnnxRuntime NuGet package and replace the stub body
/// with InferenceSession when a model is available.
/// </summary>
public class OnnxModelRunner
{
    private readonly ILogger<OnnxModelRunner> _logger;

    public OnnxModelRunner(ILogger<OnnxModelRunner> logger) => _logger = logger;

    public ModelMetadata? LoadModel(string modelPath)
    {
        if (!File.Exists(modelPath))
        {
            _logger.LogWarning("ONNX model not found at {Path}", modelPath);
            return null;
        }

        var info = new FileInfo(modelPath);
        _logger.LogInformation("Loaded ONNX model: {Path} ({Size} bytes)", modelPath, info.Length);

        return new ModelMetadata(
            Path.GetFileNameWithoutExtension(modelPath),
            1,
            modelPath,
            info.Length,
            DateTime.UtcNow);
    }

    public async Task<string?> RunInferenceAsync(string modelPath, string prompt, CancellationToken ct = default)
    {
        if (!File.Exists(modelPath))
        {
            _logger.LogWarning("ONNX inference skipped – model not found at {Path}", modelPath);
            return null;
        }

        // TODO: replace with Microsoft.ML.OnnxRuntime InferenceSession
        // var session = new InferenceSession(modelPath);
        // var inputs = BuildInputTensors(prompt);
        // var outputs = session.Run(inputs);
        // return DecodeOutputTokens(outputs);

        await Task.CompletedTask;
        return null;
    }
}
