using Benchmark.Domain.Problems.Common;
using ErrorOr;

namespace Benchmark.Domain.Problems;

public class CodingProblem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ProblemCategory Category { get; private set; }
    public DifficultyLevel Difficulty { get; private set; }
    public string FunctionSignature { get; private set; } = string.Empty;  // f.e. "int[] TwoSum(int[] nums, int target)"
    public IReadOnlyList<TestCase> TestCases { get; private set; } = new List<TestCase>();

    private readonly List<TestCase> _testCases = new();

    // private ctor
    private CodingProblem() { }

    public static ErrorOr<CodingProblem> Create(
        string title,
        string description,
        ProblemCategory category,
        DifficultyLevel difficulty,
        string functionSignature,
        IEnumerable<TestCase> testCases)
    {
        if (string.IsNullOrWhiteSpace(title))
            return DomainErrors.Problem.InvalidTitle;

        if (string.IsNullOrWhiteSpace(description))
            return DomainErrors.Problem.InvalidDescription;

        if (!testCases.Any())
            return DomainErrors.Problem.NoTestCases;

        var problem = new CodingProblem
        {
            Id = Guid.NewGuid(),
            Title = title.Trim(),
            Description = description.Trim(),
            Category = category,
            Difficulty = difficulty,
            FunctionSignature = functionSignature.Trim(),
        };

        foreach (var tc in testCases)
            problem._testCases.Add(tc);

        problem.TestCases = problem._testCases.AsReadOnly();

        return problem;
    }
}