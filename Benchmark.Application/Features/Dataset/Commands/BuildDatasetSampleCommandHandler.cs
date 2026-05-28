using Benchmark.Application.Common.DTOs;
using Benchmark.Application.Dataset;
using Benchmark.Domain.Dataset;
using Benchmark.Domain.Problems.Common;
using Benchmark.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Dataset.Commands;

public class BuildDatasetSampleCommandHandler
    : IRequestHandler<BuildDatasetSampleCommand, ErrorOr<FineTuneSampleDto>>
{
    private readonly IEvaluationRepository _evaluations;
    private readonly IProblemRepository _problems;
    private readonly IFineTuneSampleRepository _samples;
    private readonly IUnitOfWork _unitOfWork;

    public BuildDatasetSampleCommandHandler(
        IEvaluationRepository evaluations,
        IProblemRepository problems,
        IFineTuneSampleRepository samples,
        IUnitOfWork unitOfWork)
    {
        _evaluations = evaluations;
        _problems    = problems;
        _samples     = samples;
        _unitOfWork  = unitOfWork;
    }

    public async Task<ErrorOr<FineTuneSampleDto>> Handle(
        BuildDatasetSampleCommand request, CancellationToken ct)
    {
        var evaluation = await _evaluations.GetByIdAsync(request.EvaluationId, ct);
        if (evaluation is null)
            return DomainErrors.Evaluation.NotFound;

        if (evaluation.GeneratedCode is null)
            return DomainErrors.Evaluation.NoCode;

        var problem = await _problems.GetByIdAsync(evaluation.ProblemId, ct);
        if (problem is null)
            return DomainErrors.Problem.NotFound;

        var sample = DatasetBuilder.Build(problem, evaluation)!;

        await _samples.AddAsync(sample, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return ToDto(sample);
    }

    private static FineTuneSampleDto ToDto(FineTuneSample s)
        => new(s.Id, s.ProblemId, s.EvaluationId, s.ProblemTitle,
               s.Category.ToString(), s.Difficulty.ToString(),
               s.ModelName, s.SampleType.ToString(), s.Verdict.ToString(),
               s.Score, s.CreatedAt);
}
