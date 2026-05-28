using Benchmark.Application.Features.Training.Commands;
using Benchmark.Application.Features.Training.Queries;
using MediatR;

namespace Benchmark.Api.Features.Training;

public static class TrainingEndpoints
{
    public static IEndpointRouteBuilder MapTrainingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/training").WithTags("Training");

        group.MapPost("/trigger", async (ISender sender) =>
        {
            var result = await sender.Send(new TriggerTrainingCommand());
            return result.Match(
                dto    => Results.Ok(dto),
                errors => Results.Problem(errors.First().Description));
        })
        .WithName("TriggerTraining")
        .WithOpenApi();

        group.MapGet("/versions", async (ISender sender) =>
            Results.Ok(await sender.Send(new GetModelVersionsQuery())))
        .WithName("GetModelVersions")
        .WithOpenApi();

        return app;
    }
}
