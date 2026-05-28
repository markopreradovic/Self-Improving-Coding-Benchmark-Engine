using Benchmark.Application.Features.Metrics.Queries;
using MediatR;

namespace Benchmark.Api.Features.Metrics;

public static class MetricsEndpoints
{
    public static IEndpointRouteBuilder MapMetricsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/metrics").WithTags("Metrics");

        group.MapGet("/summary", async (ISender sender) =>
            Results.Ok(await sender.Send(new GetMetricsSummaryQuery())))
        .WithName("GetMetricsSummary")
        .WithOpenApi();

        return app;
    }
}
