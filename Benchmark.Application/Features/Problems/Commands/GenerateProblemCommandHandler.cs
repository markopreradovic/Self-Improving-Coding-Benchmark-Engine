using Benchmark.Application.Common.DTOs;
using Benchmark.Application.Generators;
using Benchmark.Domain.Problems;
using Benchmark.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Problems.Commands;

public class GenerateProblemCommandHandler : IRequestHandler<GenerateProblemCommand, ErrorOr<ProblemDetailDto>>
{
    private readonly IProblemGenerator _generator;
    private readonly IProblemRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public GenerateProblemCommandHandler(
        IProblemGenerator generator,
        IProblemRepository repository,
        IUnitOfWork unitOfWork)
    {
        _generator = generator;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ProblemDetailDto>> Handle(GenerateProblemCommand request, CancellationToken ct)
    {
        var generated = _generator.Generate(
            request.Category ?? ProblemCategory.Array,
            request.Difficulty,
            request.MinTestCases ?? 3);

        if (generated.IsError)
            return generated.Errors;

        var problem = generated.Value;

        await _repository.AddAsync(problem, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return new ProblemDetailDto(
            problem.Id,
            problem.Title,
            problem.Description,
            problem.Category,
            problem.Difficulty,
            problem.FunctionSignature,
            problem.TestCases
                .Select(tc => new TestCaseDto(tc.Input, tc.ExpectedOutput, tc.IsHidden, tc.Explanation))
                .ToList(),
            problem.CreatedAt);
    }
}