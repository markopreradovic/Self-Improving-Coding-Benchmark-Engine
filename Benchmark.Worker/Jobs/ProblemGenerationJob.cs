using Benchmark.Application.Features.Problems.Commands;
using Benchmark.Application.Generators;
using Benchmark.Domain.Problems;
using MediatR;
using Microsoft.Extensions.Options;

namespace Benchmark.Worker.Jobs;

public class ProblemGenerationJob : IJob
{
    private static readonly ProblemCategory[] Categories =
        Enum.GetValues<ProblemCategory>().Where(c => c != ProblemCategory.Unknown).ToArray();

    private static readonly DifficultyLevel[] Difficulties = Enum.GetValues<DifficultyLevel>();

    private readonly ISender _sender;
    private readonly WorkerOptions _options;
    private readonly ILogger<ProblemGenerationJob> _logger;

    public ProblemGenerationJob(ISender sender, IOptions<WorkerOptions> options, ILogger<ProblemGenerationJob> logger)
    {
        _sender  = sender;
        _options = options.Value;
        _logger  = logger;
    }

    public async Task RunAsync(CancellationToken ct)
    {
        _logger.LogInformation("ProblemGenerationJob: generating {Count} problems", _options.ProblemGenerationBatchSize);

        for (int i = 0; i < _options.ProblemGenerationBatchSize; i++)
        {
            var category   = Categories[i % Categories.Length];
            var difficulty = Difficulties[i % Difficulties.Length];

            var result = await _sender.Send(new GenerateProblemCommand(category, difficulty, 3), ct);

            if (result.IsError)
                _logger.LogWarning("ProblemGenerationJob: failed [{Category}/{Difficulty}]: {Error}",
                    category, difficulty, result.FirstError.Description);
            else
                _logger.LogInformation("ProblemGenerationJob: created {Id} – {Title}",
                    result.Value.Id, result.Value.Title);
        }
    }
}
