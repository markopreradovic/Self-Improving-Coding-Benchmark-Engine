using Benchmark.Application.Common.DTOs;
using Benchmark.Domain.Problems;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Problems.Queries;

public record GetProblemsQuery(
    ProblemCategory? Category = null,
    DifficultyLevel? Difficulty = null,
    int Page = 1,
    int PageSize = 20) : IRequest<ErrorOr<PagedResult<ProblemSummaryDto>>>;
