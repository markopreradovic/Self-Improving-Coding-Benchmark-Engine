using Benchmark.Application.Common.DTOs;
using Benchmark.Domain.Repositories;
using MediatR;

namespace Benchmark.Application.Features.Metrics.Queries;

public record GetMetricsSummaryQuery : IRequest<MetricsSummaryDto>;

public class GetMetricsSummaryQueryHandler : IRequestHandler<GetMetricsSummaryQuery, MetricsSummaryDto>
{
    private readonly IMetricsRepository _metrics;

    public GetMetricsSummaryQueryHandler(IMetricsRepository metrics) => _metrics = metrics;

    public async Task<MetricsSummaryDto> Handle(GetMetricsSummaryQuery request, CancellationToken ct)
    {
        var totalProblems    = await _metrics.GetTotalProblemsAsync(ct);
        var totalEvaluations = await _metrics.GetTotalEvaluationsAsync(ct);
        var totalSamples     = await _metrics.GetTotalSamplesAsync(ct);
        var successRate      = await _metrics.GetOverallSuccessRateAsync(ct);
        var heatmap          = await _metrics.GetHeatmapDataAsync(ct);

        return new MetricsSummaryDto(
            totalProblems,
            totalEvaluations,
            totalSamples,
            successRate,
            heatmap.Select(h => new HeatmapEntryDto(h.Category, h.Difficulty, h.Total, h.AvgScore)).ToList());
    }
}
