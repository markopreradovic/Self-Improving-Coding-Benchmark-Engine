using Benchmark.Application.Common.DTOs;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Evaluation.Commands;

public record EvaluateAttemptCommand(
    Guid ProblemId,
    string Code,
    string ModelName) : IRequest<ErrorOr<EvaluationResultDto>>;
