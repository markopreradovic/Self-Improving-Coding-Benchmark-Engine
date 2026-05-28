using Benchmark.Application.Common.DTOs;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Training.Commands;

public record TriggerTrainingCommand : IRequest<ErrorOr<ModelVersionDto>>;
