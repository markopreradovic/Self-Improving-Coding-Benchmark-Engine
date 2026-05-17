using Benchmark.Application.Common.DTOs;
using Benchmark.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Problems.Queries;

public class GetProblemsQueryHandler : IRequestHandler<GetProblemsQuery, ErrorOr<PagedResult<ProblemSummaryDto>>>
{
    private readonly IProblemRepository _repository;

    public GetProblemsQueryHandler(IProblemRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<PagedResult<ProblemSummaryDto>>> Handle(GetProblemsQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(
            request.Category,
            request.Difficulty,
            request.Page,
            request.PageSize,
            ct);

        var dtos = items
            .Select(p => new ProblemSummaryDto(
                p.Id,
                p.Title,
                p.Category,
                p.Difficulty,
                p.TestCases.Count,
                p.CreatedAt))
            .ToList();

        return new PagedResult<ProblemSummaryDto>(dtos, totalCount, request.Page, request.PageSize);
    }
}
