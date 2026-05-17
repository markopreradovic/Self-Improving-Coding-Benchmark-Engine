using Benchmark.Application.Features.Problems.Commands;
using Benchmark.Application.Features.Problems.Queries;
using Benchmark.Domain.Problems;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Benchmark.Api.Features.Problems;

public static class ProblemEndpoints
{
    public static IEndpointRouteBuilder MapProblemEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/problems")
            .WithTags("Problems")
            .WithOpenApi();

        group.MapPost("/generate", async (
                [FromBody] GenerateProblemRequest request,
                ISender mediator,
                CancellationToken ct) =>
            {
                var command = new GenerateProblemCommand(
                    Category: request.Category,
                    Difficulty: request.Difficulty,
                    MinTestCases: request.MinTestCases ?? 3);

                var result = await mediator.Send(command, ct);

                return result.Match(
                    dto => Results.Ok(dto),
                    errors => Results.BadRequest(errors));
            })
            .WithName("GenerateProblem")
            .WithSummary("Generate and persist a new coding problem");

        group.MapGet("/{id:guid}", async (
                Guid id,
                ISender mediator,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetProblemByIdQuery(id), ct);

                return result.Match(
                    dto => Results.Ok(dto),
                    errors => errors.Any(e => e.Type == ErrorType.NotFound)
                        ? Results.NotFound()
                        : Results.BadRequest(errors));
            })
            .WithName("GetProblemById")
            .WithSummary("Get a problem with full details and test cases");

        group.MapGet("/", async (
                [FromQuery] ProblemCategory? category,
                [FromQuery] DifficultyLevel? difficulty,
                [FromQuery] int page,
                [FromQuery] int pageSize,
                ISender mediator,
                CancellationToken ct) =>
            {
                var query = new GetProblemsQuery(
                    Category: category,
                    Difficulty: difficulty,
                    Page: page <= 0 ? 1 : page,
                    PageSize: pageSize <= 0 ? 20 : Math.Min(pageSize, 100));

                var result = await mediator.Send(query, ct);

                return result.Match(
                    dto => Results.Ok(dto),
                    errors => Results.BadRequest(errors));
            })
            .WithName("GetProblems")
            .WithSummary("Get paginated list of problems with optional filters");

        return endpoints;
    }
}

public record GenerateProblemRequest(
    ProblemCategory? Category = null,
    DifficultyLevel? Difficulty = null,
    int? MinTestCases = null);
