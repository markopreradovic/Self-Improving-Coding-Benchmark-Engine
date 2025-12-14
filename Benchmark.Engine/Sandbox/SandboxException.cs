namespace Benchmark.Engine.Sandbox;

public class SandboxException : Exception
{
    public SandboxException(string message, Exception? inner = null)
        : base(message, inner)
    {
    }
}
