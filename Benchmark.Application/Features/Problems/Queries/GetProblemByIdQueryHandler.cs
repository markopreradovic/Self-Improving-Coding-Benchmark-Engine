using Benchmark.Application.Common.DTOs;
using Benchmark.Domain.Problems.Common;
using Benchmark.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Problems.Queries;

public class GetProblemByIdQueryHandler : IRequestHandler<GetProblemByIdQuery, ErrorOr<ProblemDetailDto>>
{
    private readonly IProblemRepository _repository;

    public GetProblemByIdQueryHandler(IProblemRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<ProblemDetailDto>> Handle(GetProblemByIdQuery request, CancellationToken ct)
    {
        var problem = await _repository.GetByIdAsync(request.Id, ct);

        if (problem is null)
            return DomainErrors.Problem.NotFound;

        var dto = new ProblemDetailDto(
            problem.Id,
            problem.Title,
            problem.Description,
            problem.Category,
            problem.Difficulty,
            problem.FunctionSignature,
            problem.TestCases
                .Select(tc => new TestCaseDto(tc.Input, tc.ExpectedOutput, tc.IsHidden, tc.Explanation))
                .ToList(),
            problem.CreatedAt);

        return dto;
    }
}
