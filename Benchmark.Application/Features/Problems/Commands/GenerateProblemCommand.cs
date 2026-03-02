using Benchmark.Domain.Problems;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Problems.Commands;

public record GenerateProblemCommand(
    ProblemCategory? Category = null,         
    DifficultyLevel? Difficulty = null,
    int? MinTestCases = 3
) : IRequest<ErrorOr<CodingProblem>>;