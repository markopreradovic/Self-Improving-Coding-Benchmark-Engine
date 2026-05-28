using Benchmark.Application.Features.Evaluation.Commands;
using Benchmark.Application.Features.Solver.Commands;
using Benchmark.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;

namespace Benchmark.Worker.Jobs;

public class EvaluationJob : IJob
{
    private readonly IProblemRepository _problems;
    private readonly ISender _sender;
    private readonly WorkerOptions _options;
    private readonly ILogger<EvaluationJob> _logger;

    public EvaluationJob(
        IProblemRepository problems,
        ISender sender,
        IOptions<WorkerOptions> options,
        ILogger<EvaluationJob> logger)
    {
        _problems = problems;
        _sender   = sender;
        _options  = options.Value;
        _logger   = logger;
    }

    public async Task RunAsync(CancellationToken ct)
    {
        var unevaluated = await _problems.GetUnevaluatedAsync(_options.EvaluationBatchSize, ct);
        _logger.LogInformation("EvaluationJob: {Count} unevaluated problems", unevaluated.Count);

        foreach (var problem in unevaluated)
        {
            var solveResult = await _sender.Send(new SolveProblemCommand(problem.Id), ct);

            if (solveResult.IsError || solveResult.Value.GeneratedCode is null)
            {
                _logger.LogWarning("EvaluationJob: solve failed for problem {Id}", problem.Id);
                continue;
            }

            var evalResult = await _sender.Send(new EvaluateAttemptCommand(
                problem.Id,
                solveResult.Value.GeneratedCode,
                solveResult.Value.ModelName), ct);

            if (evalResult.IsError)
                _logger.LogWarning("EvaluationJob: evaluation failed for problem {Id}: {Error}",
                    problem.Id, evalResult.FirstError.Description);
            else
                _logger.LogInformation("EvaluationJob: problem {Id} → {Verdict} ({Passed}/{Total}) score={Score:P0}",
                    problem.Id, evalResult.Value.OverallVerdict,
                    evalResult.Value.PassedCount, evalResult.Value.TotalCount,
                    evalResult.Value.Score);
        }
    }
}
