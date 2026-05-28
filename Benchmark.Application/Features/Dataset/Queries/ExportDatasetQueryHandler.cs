using Benchmark.Application.Dataset;
using Benchmark.Domain.Repositories;
using MediatR;

namespace Benchmark.Application.Features.Dataset.Queries;

public class ExportDatasetQueryHandler : IRequestHandler<ExportDatasetQuery, string>
{
    private readonly IFineTuneSampleRepository _samples;

    public ExportDatasetQueryHandler(IFineTuneSampleRepository samples) => _samples = samples;

    public async Task<string> Handle(ExportDatasetQuery request, CancellationToken ct)
    {
        var samples = await _samples.GetFilteredAsync(
            request.Category,
            request.Difficulty,
            request.SampleType,
            request.MinScore,
            ct);

        return DatasetExporter.ExportToJsonl(samples, request.Format);
    }
}
