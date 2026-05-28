using Benchmark.Application.Features.Dataset.Commands;
using Benchmark.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;

namespace Benchmark.Worker.Jobs;

public class TrainingJob : IJob
{
    private readonly IEvaluationRepository _evaluations;
    private readonly ISender _sender;
    private readonly WorkerOptions _options;
    private readonly ILogger<TrainingJob> _logger;

    public TrainingJob(
        IEvaluationRepository evaluations,
        ISender sender,
        IOptions<WorkerOptions> options,
        ILogger<TrainingJob> logger)
    {
        _evaluations = evaluations;
        _sender      = sender;
        _options     = options.Value;
        _logger      = logger;
    }

    public async Task RunAsync(CancellationToken ct)
    {
        var evaluations = await _evaluations.GetWithoutSamplesAsync(_options.DatasetBatchSize, ct);
        _logger.LogInformation("TrainingJob: building samples for {Count} evaluations", evaluations.Count);

        int built = 0;
        foreach (var evaluation in evaluations)
        {
            var result = await _sender.Send(new BuildDatasetSampleCommand(evaluation.Id), ct);
            if (result.IsError)
                _logger.LogWarning("TrainingJob: failed for evaluation {Id}: {Error}",
                    evaluation.Id, result.FirstError.Description);
            else
                built++;
        }

        _logger.LogInformation("TrainingJob: built {Count}/{Total} samples", built, evaluations.Count);
    }
}
