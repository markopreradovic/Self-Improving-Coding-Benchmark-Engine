using Benchmark.Application.Generators;
using Benchmark.Domain.Problems;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Problems.Commands;

public class GenerateProblemCommandHandler : IRequestHandler<GenerateProblemCommand, ErrorOr<CodingProblem>>
{
    private readonly IProblemGenerator _generator;

    public GenerateProblemCommandHandler(IProblemGenerator generator)
    {
        _generator = generator;
    }
    public async Task<ErrorOr<CodingProblem>> Handle(GenerateProblemCommand request, CancellationToken ct)
    {
        // Koristi generator
        var generated = _generator.Generate(
            request.Category ?? ProblemCategory.Array, // default Array za test
            request.Difficulty,
            request.MinTestCases ?? 3);

        return generated;
    }
}