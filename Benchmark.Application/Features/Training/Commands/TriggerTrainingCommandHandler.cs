using Benchmark.Application.Common.DTOs;
using Benchmark.Domain.Repositories;
using Benchmark.Domain.Training;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Training.Commands;

public class TriggerTrainingCommandHandler
    : IRequestHandler<TriggerTrainingCommand, ErrorOr<ModelVersionDto>>
{
    private readonly ITrainingOrchestrator _orchestrator;
    private readonly IUnitOfWork _unitOfWork;

    public TriggerTrainingCommandHandler(ITrainingOrchestrator orchestrator, IUnitOfWork unitOfWork)
    {
        _orchestrator = orchestrator;
        _unitOfWork   = unitOfWork;
    }

    public async Task<ErrorOr<ModelVersionDto>> Handle(TriggerTrainingCommand request, CancellationToken ct)
    {
        var result = await _orchestrator.TriggerAsync(ct);

        if (!result.Success || result.ModelVersion is null)
            return Error.Failure("Training.Failed", result.Message);

        await _unitOfWork.SaveChangesAsync(ct);

        var v = result.ModelVersion;
        return new ModelVersionDto(v.Id, v.ModelName, v.VersionNumber, v.BaseModel,
            v.FilePath, v.AccuracyScore, v.TrainingSamples, v.IsActive, v.CreatedAt);
    }
}
