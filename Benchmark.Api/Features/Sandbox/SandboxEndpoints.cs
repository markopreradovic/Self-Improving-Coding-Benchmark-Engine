using Benchmark.Domain.Sandbox;

namespace Benchmark.Api.Features.Sandbox;

public static class SandboxEndpoints
{
    public static IEndpointRouteBuilder MapSandboxEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sandbox").WithTags("Sandbox");

        group.MapPost("/execute", async (
            SandboxExecuteRequest request,
            ICodeRunner runner,
            CancellationToken ct) =>
        {
            var limits = new SandboxLimits { TimeLimit = TimeSpan.FromSeconds(request.TimeoutSeconds) };
            var result = await runner.RunAsync(request.Code, request.Input, limits, ct);

            return Results.Ok(new SandboxExecuteResponse(
                result.Status.ToString(),
                result.Output,
                result.ErrorMessage,
                result.ExecutionTime.TotalMilliseconds));
        })
        .WithSummary("Execute C# code in the sandbox")
        .WithDescription("Runs arbitrary C# script code and returns stdout, execution time, and status.");

        return app;
    }
}

public record SandboxExecuteRequest(
    string Code,
    string? Input = null,
    int TimeoutSeconds = 5);

public record SandboxExecuteResponse(
    string Status,
    string? Output,
    string? ErrorMessage,
    double ExecutionTimeMs);
