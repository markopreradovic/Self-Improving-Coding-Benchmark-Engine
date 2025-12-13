namespace Benchmark.Engine.Dataset;

public class FineTuneSample
{
    /// <summary>
    /// Input which model will receive
    /// </summary>
    public string Prompt { get; set; } = string.Empty;

    /// <summary>
    /// Expected model output
    /// </summary>
    public string Completion { get; set; } = string.Empty;

    /// <summary>
    /// Problem category
    /// </summary>
    public string Category { get; set; } = string.Empty;
}
