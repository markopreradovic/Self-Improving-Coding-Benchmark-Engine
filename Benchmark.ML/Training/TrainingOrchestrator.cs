using Benchmark.Application.Dataset;
using Benchmark.Domain.Dataset;
using Benchmark.Domain.Repositories;
using Benchmark.Domain.Training;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Benchmark.ML.Training;

public class TrainingOrchestrator : ITrainingOrchestrator
{
    private readonly IFineTuneSampleRepository _samples;
    private readonly IModelVersionRepository _versions;
    private readonly PythonBridge _pythonBridge;
    private readonly TrainingConfig _config;
    private readonly ILogger<TrainingOrchestrator> _logger;

    public TrainingOrchestrator(
        IFineTuneSampleRepository samples,
        IModelVersionRepository versions,
        PythonBridge pythonBridge,
        IOptions<TrainingConfig> config,
        ILogger<TrainingOrchestrator> logger)
    {
        _samples     = samples;
        _versions    = versions;
        _pythonBridge = pythonBridge;
        _config      = config.Value;
        _logger      = logger;
    }

    public async Task<TrainingResult> TriggerAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("TrainingOrchestrator: starting pipeline");

        var positiveSamples = await _samples.GetFilteredAsync(
            sampleType: SampleType.Positive, minScore: 1.0, ct: ct);

        if (positiveSamples.Count < _config.MinSamplesRequired)
        {
            var msg = $"Not enough positive samples: {positiveSamples.Count}/{_config.MinSamplesRequired} required.";
            _logger.LogWarning("TrainingOrchestrator: {Message}", msg);
            return new TrainingResult(false, null, msg);
        }

        // Export dataset to JSONL
        Directory.CreateDirectory(_config.DatasetDirectory);
        var timestamp   = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        var datasetPath = Path.Combine(_config.DatasetDirectory, $"dataset_{timestamp}.jsonl");
        var jsonl       = DatasetExporter.ExportToJsonl(positiveSamples, ExportFormat.HuggingFace);
        await File.WriteAllTextAsync(datasetPath, jsonl, ct);
        _logger.LogInformation("TrainingOrchestrator: exported {Count} samples to {Path}",
            positiveSamples.Count, datasetPath);

        // Run fine-tuning
        var outputDir = Path.Combine(_config.ModelsDirectory, $"model_{timestamp}");
        var trainingResult = await _pythonBridge.RunTrainingAsync(datasetPath, outputDir, ct);

        if (!trainingResult.Success)
            return new TrainingResult(false, null, trainingResult.Message);

        // Register new model version
        var nextVersion = await _versions.GetNextVersionNumberAsync(_config.BaseModelName, ct);
        var modelPath   = Path.Combine(outputDir, "model.onnx");

        var modelVersion = ModelVersion.Create(
            modelName       : _config.BaseModelName,
            versionNumber   : nextVersion,
            baseModel       : _config.BaseModelName,
            filePath        : File.Exists(modelPath) ? modelPath : null,
            accuracyScore   : trainingResult.Accuracy,
            trainingSamples : positiveSamples.Count);

        await _versions.AddAsync(modelVersion, ct);

        _logger.LogInformation("TrainingOrchestrator: registered model v{Version} (accuracy={Accuracy:P0})",
            nextVersion, trainingResult.Accuracy);

        return new TrainingResult(true, modelVersion, trainingResult.Message);
    }
}
