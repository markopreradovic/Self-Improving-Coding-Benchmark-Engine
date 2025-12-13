namespace Benchmark.Engine.Problems.Models
{
    public class CodingProblem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Difficulty { get; set; } = "Easy";
        public List<TestCase> TestCases { get; set; } = new();
        public string ExpectedFunctionSignature { get; set; } = string.Empty;
        public string Category { get; set; } = "General";
    }
}
