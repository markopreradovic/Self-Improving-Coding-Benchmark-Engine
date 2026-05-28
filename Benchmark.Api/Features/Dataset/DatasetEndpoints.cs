using System.Text;
using Benchmark.Application.Features.Dataset.Commands;
using Benchmark.Application.Features.Dataset.Queries;
using Benchmark.Domain.Dataset;
using Benchmark.Domain.Problems;
using MediatR;

namespace Benchmark.Api.Features.Dataset;

public static class DatasetEndpoints
{
    public static IEndpointRouteBuilder MapDatasetEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dataset").WithTags("Dataset");

        group.MapPost("/build", async (BuildDatasetSampleCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.Match(
                dto => Results.Ok(dto),
                errors => Results.Problem(errors.First().Description));
        })
        .WithName("BuildDatasetSample")
        .WithOpenApi();

        group.MapGet("/export", async (
            ISender sender,
            ExportFormat format = ExportFormat.OpenAI,
            ProblemCategory? category = null,
            DifficultyLevel? difficulty = null,
            SampleType? sampleType = null,
            double minScore = 0.0) =>
        {
            var jsonl = await sender.Send(
                new ExportDatasetQuery(format, category, difficulty, sampleType, minScore));

            var fileName = $"dataset_{format}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.jsonl";
            return Results.File(Encoding.UTF8.GetBytes(jsonl), "application/jsonl", fileName);
        })
        .WithName("ExportDataset")
        .WithOpenApi();

        return app;
    }
}
