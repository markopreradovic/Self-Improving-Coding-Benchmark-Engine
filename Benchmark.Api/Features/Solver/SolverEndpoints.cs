using Benchmark.Application.Features.Solver.Commands;
using MediatR;

namespace Benchmark.Api.Features.Solver;

public static class SolverEndpoints
{
    public static IEndpointRouteBuilder MapSolverEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/solver").WithTags("Solver");

        group.MapPost("/solve", async (
            SolveProblemRequest request,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new SolveProblemCommand(request.ProblemId), ct);
            return result.Match(
                dto => Results.Ok(dto),
                errors => Results.BadRequest(errors));
        })
        .WithSummary("Solve a problem using the configured LLM")
        .WithDescription("Fetches the problem by ID, calls the LLM solver, and returns the generated C# code.");

        return app;
    }
}

public record SolveProblemRequest(Guid ProblemId);
