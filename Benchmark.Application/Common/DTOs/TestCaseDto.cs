namespace Benchmark.Application.Common.DTOs;

public record TestCaseDto(
    string Input,
    string ExpectedOutput,
    bool IsHidden,
    string? Explanation);
