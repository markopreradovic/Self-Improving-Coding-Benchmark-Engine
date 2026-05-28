using Benchmark.Application.Common.DTOs;
using Benchmark.Domain.Repositories;
using MediatR;

namespace Benchmark.Application.Features.Training.Queries;

public record GetModelVersionsQuery : IRequest<IReadOnlyList<ModelVersionDto>>;

public class GetModelVersionsQueryHandler : IRequestHandler<GetModelVersionsQuery, IReadOnlyList<ModelVersionDto>>
{
    private readonly IModelVersionRepository _repository;

    public GetModelVersionsQueryHandler(IModelVersionRepository repository) => _repository = repository;

    public async Task<IReadOnlyList<ModelVersionDto>> Handle(GetModelVersionsQuery request, CancellationToken ct)
    {
        var versions = await _repository.GetAllAsync(ct);
        return versions.Select(v => new ModelVersionDto(
            v.Id, v.ModelName, v.VersionNumber, v.BaseModel,
            v.FilePath, v.AccuracyScore, v.TrainingSamples, v.IsActive, v.CreatedAt))
            .ToList();
    }
}
