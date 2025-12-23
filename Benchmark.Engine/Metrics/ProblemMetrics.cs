namespace Benchmark.Engine.Metrics;

public class ProblemMetrics
{
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public int PassedTests { get; set; }
    public int TotalTests { get; set; }
    public double Accuracy { get; set; }
    public int TrainingIterations { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
