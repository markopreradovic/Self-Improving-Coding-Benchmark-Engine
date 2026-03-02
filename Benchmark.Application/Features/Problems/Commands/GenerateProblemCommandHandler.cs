using Benchmark.Domain.Problems;
using Benchmark.Domain.Problems.Common;
using ErrorOr;
using MediatR;

namespace Benchmark.Application.Features.Problems.Commands;

public class GenerateProblemCommandHandler : IRequestHandler<GenerateProblemCommand, ErrorOr<CodingProblem>>
{
    public async Task<ErrorOr<CodingProblem>> Handle(
        GenerateProblemCommand request,
        CancellationToken cancellationToken)
    {

        var testCases = new List<TestCase>
        {
            TestCase.Create("[2,7,11,15]", "[0,1]", false, "Because nums[0] + nums[1] = 9"),
            TestCase.Create("[3,2,4]", "[1,2]", false),
            TestCase.Create("[3,3]", "[0,1]", false)
        };

        var result = CodingProblem.Create(
            title: "Two Sum",
            description:
            "Given an array of integers nums and an integer target, return indices of the two numbers such that they add up to target.",
            category: ProblemCategory.Array,
            difficulty: DifficultyLevel.Medium,
            functionSignature: "int[] TwoSum(int[] nums, int target)",
            testCases: testCases
        );

        if (request.Category.HasValue && result.Value.Category != request.Category.Value)
        {
            return DomainErrors.Problem.InvalidCategory;

        }
        
        return result;
    }
}