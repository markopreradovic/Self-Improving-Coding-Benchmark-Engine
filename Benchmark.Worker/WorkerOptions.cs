namespace Benchmark.Worker;

public class WorkerOptions
{
    public const string Section = "Worker";

    public int ProblemGenerationBatchSize { get; set; } = 3;
    public int EvaluationBatchSize { get; set; } = 3;
    public int DatasetBatchSize { get; set; } = 5;
    public int JobIntervalSeconds { get; set; } = 300;
}
