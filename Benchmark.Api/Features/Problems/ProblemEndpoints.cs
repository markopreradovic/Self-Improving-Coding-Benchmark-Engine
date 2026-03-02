using Benchmark.Application.Features.Problems.Commands;
using Benchmark.Domain.Problems;
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
                    MinTestCases: request.MinTestCases ?? 3
                );

                var result = await mediator.Send(command, ct);

                return result.Match(
                    problem => Results.Ok(new
                    {
                        Id = problem.Id,
                        Title = problem.Title,
                        Category = problem.Category
                    }),
                    errors => Results.BadRequest(errors)
                );
            })
            .WithName("GenerateProblem");

        // ovdje kasnije možeš dodati još ruta za istu grupu
        // group.MapGet("/{id}", ...);
        // group.MapGet("/", ...);

        return endpoints;
    }
}

// DTO može ostati ovdje ili premjestiti u Benchmark.Application / DTOs
public record GenerateProblemRequest(
    ProblemCategory? Category = null,
    DifficultyLevel? Difficulty = null,
    int? MinTestCases = null);