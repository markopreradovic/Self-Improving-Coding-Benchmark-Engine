using Benchmark.Application.Common.DTOs;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Dataset.Commands;

public record BuildDatasetSampleCommand(Guid EvaluationId) : IRequest<ErrorOr<FineTuneSampleDto>>;
