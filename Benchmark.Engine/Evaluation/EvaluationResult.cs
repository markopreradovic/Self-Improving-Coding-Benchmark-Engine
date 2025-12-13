namespace Benchmark.Engine.Evaluation
{
    public class EvaluationResult
    {
        public string TestInput { get; set; } = string.Empty;
        public string ExpectedOutput { get; set; } = string.Empty;
        public string ActualOutput { get; set; } = string.Empty;
        public bool Passed { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;


    }
}
