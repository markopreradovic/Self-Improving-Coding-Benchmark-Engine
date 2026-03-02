namespace Benchmark.Domain.Problems;

public class TestCase
{
    public string Input { get; private set; } = string.Empty;      // JSON or string
    public string ExpectedOutput { get; private set; } = string.Empty;
    public bool IsHidden { get; private set; }                     
    public string? Explanation { get; private set; }               // optional
    
    private TestCase() { }

    public static TestCase Create(string input, string expectedOutput, bool isHidden = false, string? explanation = null)
    {
        return new TestCase
        {
            Input = input,
            ExpectedOutput = expectedOutput,
            IsHidden = isHidden,
            Explanation = explanation
        };
    }
}