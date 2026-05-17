using Benchmark.Domain.Problems;

namespace Benchmark.Application.Common.DTOs;

public record ProblemSummaryDto(
    Guid Id,
    string Title,
    ProblemCategory Category,
    DifficultyLevel Difficulty,
    int TestCaseCount,
    DateTime CreatedAt);
