using Benchmark.Domain.Problems;

namespace Benchmark.Application.Common.DTOs;

public record ProblemDetailDto(
    Guid Id,
    string Title,
    string Description,
    ProblemCategory Category,
    DifficultyLevel Difficulty,
    string FunctionSignature,
    IReadOnlyList<TestCaseDto> TestCases,
    DateTime CreatedAt);
