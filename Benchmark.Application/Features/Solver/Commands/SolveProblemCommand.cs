using Benchmark.Application.Common.DTOs;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Solver.Commands;

public record SolveProblemCommand(Guid ProblemId) : IRequest<ErrorOr<SolveAttemptDto>>;
