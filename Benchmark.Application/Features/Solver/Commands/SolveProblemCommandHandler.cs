using Benchmark.Application.Common.DTOs;
using Benchmark.Domain.LLM;
using Benchmark.Domain.Problems.Common;
using Benchmark.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Solver.Commands;

public class SolveProblemCommandHandler : IRequestHandler<SolveProblemCommand, ErrorOr<SolveAttemptDto>>
{
    private readonly IProblemRepository _repository;
    private readonly ISolver _solver;

    public SolveProblemCommandHandler(IProblemRepository repository, ISolver solver)
    {
        _repository = repository;
        _solver = solver;
    }

    public async Task<ErrorOr<SolveAttemptDto>> Handle(
        SolveProblemCommand request, CancellationToken ct)
    {
        var problem = await _repository.GetByIdAsync(request.ProblemId, ct);
        if (problem is null)
            return DomainErrors.Problem.NotFound;

        var attempt = await _solver.SolveAsync(problem, ct);

        return new SolveAttemptDto(
            attempt.ProblemId,
            attempt.Status.ToString(),
            attempt.GeneratedCode,
            attempt.ErrorMessage,
            attempt.ResponseTime.TotalMilliseconds,
            attempt.ModelName);
    }
}
