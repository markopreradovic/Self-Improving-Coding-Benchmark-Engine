using Benchmark.Application.Common.DTOs;
using Benchmark.Domain.Evaluation;
using Benchmark.Domain.Problems.Common;
using Benchmark.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Evaluation.Commands;

public class EvaluateAttemptCommandHandler
    : IRequestHandler<EvaluateAttemptCommand, ErrorOr<EvaluationResultDto>>
{
    private readonly IProblemRepository _problems;
    private readonly IEvaluationRepository _evaluations;
    private readonly IEvaluator _evaluator;
    private readonly IUnitOfWork _unitOfWork;

    public EvaluateAttemptCommandHandler(
        IProblemRepository problems,
        IEvaluationRepository evaluations,
        IEvaluator evaluator,
        IUnitOfWork unitOfWork)
    {
        _problems = problems;
        _evaluations = evaluations;
        _evaluator = evaluator;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<EvaluationResultDto>> Handle(
        EvaluateAttemptCommand request, CancellationToken ct)
    {
        var problem = await _problems.GetByIdAsync(request.ProblemId, ct);
        if (problem is null)
            return DomainErrors.Problem.NotFound;

        var result = await _evaluator.EvaluateAsync(problem, request.Code, request.ModelName, ct);

        await _evaluations.AddAsync(result, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return ToDto(result);
    }

    private static EvaluationResultDto ToDto(EvaluationResult result)
    {
        var analysis = FailureAnalyzer.Analyze(result);

        return new EvaluationResultDto(
            result.Id,
            result.ProblemId,
            result.ModelName,
            result.OverallVerdict.ToString(),
            result.PassedCount,
            result.TotalCount,
            result.Score,
            analysis.Summary,
            result.EvaluatedAt,
            result.TestCaseResults.Select(r => new TestCaseResultDto(
                r.Input,
                r.ExpectedOutput,
                r.ActualOutput,
                r.Verdict.ToString(),
                r.ExecutionTimeMs,
                r.ErrorMessage)).ToList());
    }
}
