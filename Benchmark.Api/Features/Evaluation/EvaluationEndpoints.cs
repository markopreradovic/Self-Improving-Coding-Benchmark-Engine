using Benchmark.Application.Features.Evaluation.Commands;
using MediatR;

namespace Benchmark.Api.Features.Evaluation;

public static class EvaluationEndpoints
{
    public static IEndpointRouteBuilder MapEvaluationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/evaluation").WithTags("Evaluation");

        group.MapPost("/evaluate", async (EvaluateAttemptCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.Match(
                dto => Results.Ok(dto),
                errors => Results.Problem(errors.First().Description));
        })
        .WithName("EvaluateAttempt")
        .WithOpenApi();

        return app;
    }
}
