using Benchmark.Application.Common.DTOs;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Problems.Queries;

public record GetProblemByIdQuery(Guid Id) : IRequest<ErrorOr<ProblemDetailDto>>;
