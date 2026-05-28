using Benchmark.Domain.Evaluation;
using Benchmark.Domain.Problems;

namespace Benchmark.Domain.Dataset;

public class FineTuneSample
{
    public Guid Id { get; private set; }
    public Guid ProblemId { get; private set; }
    public Guid EvaluationId { get; private set; }

    // Denormalized problem snapshot so export needs no joins
    public string ProblemTitle { get; private set; } = string.Empty;
    public string ProblemDescription { get; private set; } = string.Empty;
    public string FunctionSignature { get; private set; } = string.Empty;
    public ProblemCategory Category { get; private set; }
    public DifficultyLevel Difficulty { get; private set; }

    public string Code { get; private set; } = string.Empty;
    public string ModelName { get; private set; } = string.Empty;
    public SampleType SampleType { get; private set; }
    public EvaluationVerdict Verdict { get; private set; }
    public double Score { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private FineTuneSample() { }

    public static FineTuneSample Create(
        CodingProblem problem,
        Guid evaluationId,
        string code,
        string modelName,
        EvaluationVerdict verdict,
        double score)
    {
        return new FineTuneSample
        {
            Id = Guid.NewGuid(),
            ProblemId = problem.Id,
            EvaluationId = evaluationId,
            ProblemTitle = problem.Title,
            ProblemDescription = problem.Description,
            FunctionSignature = problem.FunctionSignature,
            Category = problem.Category,
            Difficulty = problem.Difficulty,
            Code = code,
            ModelName = modelName,
            SampleType = verdict == EvaluationVerdict.Accepted ? SampleType.Positive : SampleType.Negative,
            Verdict = verdict,
            Score = score,
            CreatedAt = DateTime.UtcNow
        };
    }
}
