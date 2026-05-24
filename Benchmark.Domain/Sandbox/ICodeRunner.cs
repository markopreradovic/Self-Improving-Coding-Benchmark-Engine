namespace Benchmark.Domain.Sandbox;

public interface ICodeRunner
{
    Task<ExecutionResult> RunAsync(
        string code,
        string? stdinInput = null,
        SandboxLimits? limits = null,
        CancellationToken ct = default);
}
