namespace Benchmark.Domain.Sandbox;

public record SandboxLimits
{
    public TimeSpan TimeLimit { get; init; } = TimeSpan.FromSeconds(5);

    public static readonly SandboxLimits Default = new();
}
