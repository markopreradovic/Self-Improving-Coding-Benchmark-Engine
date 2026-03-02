using Benchmark.Domain.Problems;
using Benchmark.Domain.Problems.Common;
using ErrorOr;

namespace Benchmark.Application.Generators;

public class ArrayProblemGenerator : IProblemGenerator
{
    private static readonly Random _random = new Random();

    private readonly List<(string Title, string DescriptionTemplate, string FuncSigTemplate, Func<int, List<TestCase>> TestCaseGenerator)> _templates = new()
    {
        ("Two Sum Variant", 
         "Given an array of integers {0} and an integer target, return indices of two numbers that add up to target.",
         "int[] TwoSum(int[] nums, int target)",
         count => GenerateTwoSumTestCases(count)),

        ("Contains Duplicate Variant",
         "Given an integer array {0}, return true if any value appears at least twice.",
         "bool ContainsDuplicate(int[] nums)",
         count => GenerateContainsDuplicateTestCases(count)),

        ("Best Time to Buy and Sell Stock Variant",
         "You are given an array {0} where prices[i] is the price on day i. Return the maximum profit you can achieve.",
         "int MaxProfit(int[] prices)",
         count => GenerateBuySellTestCases(count)),
    };

    public ErrorOr<CodingProblem> Generate(
        ProblemCategory category,
        DifficultyLevel? difficulty = null,
        int minTestCases = 3)
    {
        if (category != ProblemCategory.Array)
            return DomainErrors.Problem.UnsupportedCategory;

        var diff = difficulty ?? (DifficultyLevel)_random.Next(0, 3);

        var template = _templates[_random.Next(_templates.Count)];

        var arrayVarName = "nums";

        var description = string.Format(template.DescriptionTemplate, arrayVarName);
        var funcSig = template.FuncSigTemplate;

        var testCases = template.TestCaseGenerator(minTestCases + _random.Next(0, 4));

        return CodingProblem.Create(
            title: template.Title + " #" + _random.Next(1000, 9999),
            description: description,
            category: ProblemCategory.Array,
            difficulty: diff,
            functionSignature: funcSig,
            testCases: testCases
        );
    }

    private static List<TestCase> GenerateTwoSumTestCases(int count)
    {
        var cases = new List<TestCase>();

        for (int i = 0; i < count; i++)
        {
            int size = _random.Next(4, 15);
            var nums = Enumerable.Range(0, size).Select(_ => _random.Next(-100, 100)).ToArray();
            int idx1 = _random.Next(0, size);
            int idx2 = _random.Next(0, size);
            while (idx2 == idx1) idx2 = _random.Next(0, size);

            int target = nums[idx1] + nums[idx2];

            nums = nums.OrderBy(_ => _random.Next()).ToArray();

            string inputJson = $"[{string.Join(",", nums)}]";
            string expected = $"[{Array.IndexOf(nums, nums[idx1])},{Array.IndexOf(nums, nums[idx2])}]";

            cases.Add(TestCase.Create(inputJson + "," + target, expected, i >= 2));
        }

        return cases;
    }

    private static List<TestCase> GenerateContainsDuplicateTestCases(int count)
    {
        var cases = new List<TestCase>();

        for (int i = 0; i < count; i++)
        {
            int size = _random.Next(3, 12);
            bool hasDup = _random.NextDouble() > 0.3;

            var nums = Enumerable.Range(0, size).Select(_ => _random.Next(1, 20)).ToList();

            if (hasDup)
            {
                int dupIdx = _random.Next(0, size / 2);
                nums.Add(nums[dupIdx]);
            }

            string input = $"[{string.Join(",", nums)}]";
            string expected = hasDup.ToString().ToLower();

            cases.Add(TestCase.Create(input, expected, i % 2 == 1));
        }

        return cases;
    }

    private static List<TestCase> GenerateBuySellTestCases(int count)
    {
        var cases = new List<TestCase>();

        for (int i = 0; i < count; i++)
        {
            int days = _random.Next(5, 20);
            var prices = Enumerable.Range(0, days).Select(_ => _random.Next(1, 200)).ToArray();

            int minPrice = prices[0], maxProfit = 0;
            for (int j = 1; j < days; j++)
            {
                if (prices[j] < minPrice) minPrice = prices[j];
                else maxProfit = Math.Max(maxProfit, prices[j] - minPrice);
            }

            string input = $"[{string.Join(",", prices)}]";
            string expected = maxProfit.ToString();

            cases.Add(TestCase.Create(input, expected, i >= 3));
        }

        return cases;
    }
}