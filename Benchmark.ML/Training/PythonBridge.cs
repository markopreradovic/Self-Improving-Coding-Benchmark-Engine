using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Benchmark.ML.Training;

public record PythonTrainingResult(bool Success, double Accuracy, string Message);

public class PythonBridge
{
    private readonly TrainingConfig _config;
    private readonly ILogger<PythonBridge> _logger;

    public PythonBridge(IOptions<TrainingConfig> config, ILogger<PythonBridge> logger)
    {
        _config = config.Value;
        _logger = logger;
    }

    public async Task<PythonTrainingResult> RunTrainingAsync(
        string datasetPath,
        string outputDir,
        CancellationToken ct = default)
    {
        if (!File.Exists(_config.ScriptPath))
        {
            _logger.LogWarning("Training script not found at {Path}. Skipping Python fine-tuning.", _config.ScriptPath);
            return new PythonTrainingResult(false, 0, $"Script not found: {_config.ScriptPath}");
        }

        Directory.CreateDirectory(outputDir);

        var args = $"{_config.ScriptPath} " +
                   $"--dataset \"{datasetPath}\" " +
                   $"--output \"{outputDir}\" " +
                   $"--base-model \"{_config.BaseModelName}\"";

        _logger.LogInformation("Starting Python training: {Python} {Args}", _config.PythonPath, args);

        var psi = new ProcessStartInfo
        {
            FileName               = _config.PythonPath,
            Arguments              = args,
            RedirectStandardOutput = true,
            RedirectStandardError  = true,
            UseShellExecute        = false,
            CreateNoWindow         = true
        };

        using var process = new Process { StartInfo = psi };
        process.Start();

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        cts.CancelAfter(TimeSpan.FromMinutes(_config.TimeoutMinutes));

        await process.WaitForExitAsync(cts.Token);

        var stdout = await process.StandardOutput.ReadToEndAsync(ct);
        var stderr = await process.StandardError.ReadToEndAsync(ct);

        if (process.ExitCode != 0)
        {
            _logger.LogError("Training failed (exit {Code}): {Error}", process.ExitCode, stderr);
            return new PythonTrainingResult(false, 0, stderr);
        }

        _logger.LogInformation("Training completed. Output: {Output}", stdout);
        return ParseTrainingOutput(stdout);
    }

    private static PythonTrainingResult ParseTrainingOutput(string stdout)
    {
        try
        {
            var last = stdout.Trim().Split('\n').Last(l => l.TrimStart().StartsWith("{"));
            var doc  = JsonDocument.Parse(last);
            var acc  = doc.RootElement.GetProperty("accuracy").GetDouble();
            return new PythonTrainingResult(true, acc, "Training completed successfully.");
        }
        catch
        {
            return new PythonTrainingResult(true, 0, "Training completed (metrics not parseable).");
        }
    }
}
