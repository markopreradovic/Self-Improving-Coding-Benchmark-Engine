using Benchmark.Domain.Problems;
using Benchmark.Domain.Problems.Common;
using ErrorOr;

namespace Benchmark.Application.Generators;

public class DynamicProgrammingProblemGenerator : ITypedProblemGenerator
{
    private static readonly Random _random = new();

    public ProblemCategory SupportedCategory => ProblemCategory.DynamicProgramming;

    private readonly List<(string Title, string Description, string FuncSig, Func<int, List<TestCase>> TestCaseGen)> _templates;

    public DynamicProgrammingProblemGenerator()
    {
        _templates = new()
        {
            ("Climb Stairs Variant",
             "You are climbing a staircase that takes n steps to reach the top. Each time you can climb 1 or 2 steps. Return the number of distinct ways you can climb to the top.",
             "int ClimbStairs(int n)",
             GenerateClimbStairsTestCases),

            ("House Robber Variant",
             "You are a robber planning to rob houses along a street. Adjacent houses have security systems and cannot both be robbed. Given an integer array nums representing money at each house, return the maximum amount you can rob tonight.",
             "int Rob(int[] nums)",
             GenerateHouseRobberTestCases),

            ("Coin Change Variant",
             "You are given an array of coin denominations and a total amount. Return the fewest number of coins needed to make up that amount. Return -1 if that amount cannot be made up by any combination.",
             "int CoinChange(int[] coins, int amount)",
             GenerateCoinChangeTestCases),
        };
    }

    public ErrorOr<CodingProblem> Generate(
        ProblemCategory category,
        DifficultyLevel? difficulty = null,
        int minTestCases = 3)
    {
        if (category != ProblemCategory.DynamicProgramming)
            return DomainErrors.Problem.UnsupportedCategory;

        var diff = difficulty ?? (DifficultyLevel)_random.Next(0, 3);
        var template = _templates[_random.Next(_templates.Count)];
        var cases = template.TestCaseGen(minTestCases + _random.Next(0, 3));

        return CodingProblem.Create(
            title: template.Title + " #" + _random.Next(1000, 9999),
            description: template.Description,
            category: ProblemCategory.DynamicProgramming,
            difficulty: diff,
            functionSignature: template.FuncSig,
            testCases: cases);
    }

    private static List<TestCase> GenerateClimbStairsTestCases(int count)
    {
        var cases = new List<TestCase>();
        for (int i = 0; i < count; i++)
        {
            int n = _random.Next(1, 15);
            cases.Add(TestCase.Create(n.ToString(), ClimbStairs(n).ToString(), i >= 2));
        }
        return cases;
    }

    private static int ClimbStairs(int n)
    {
        if (n <= 2) return n;
        int a = 1, b = 2;
        for (int i = 3; i <= n; i++) { int c = a + b; a = b; b = c; }
        return b;
    }

    private static List<TestCase> GenerateHouseRobberTestCases(int count)
    {
        var cases = new List<TestCase>();
        for (int i = 0; i < count; i++)
        {
            var nums = Enumerable.Range(0, _random.Next(2, 8))
                .Select(_ => _random.Next(1, 100))
                .ToArray();
            cases.Add(TestCase.Create(
                $"[{string.Join(",", nums)}]",
                HouseRobber(nums).ToString(),
                i >= 2));
        }
        return cases;
    }

    private static int HouseRobber(int[] nums)
    {
        if (nums.Length == 1) return nums[0];
        int prev2 = nums[0], prev1 = Math.Max(nums[0], nums[1]);
        for (int i = 2; i < nums.Length; i++)
        {
            int curr = Math.Max(prev1, prev2 + nums[i]);
            prev2 = prev1;
            prev1 = curr;
        }
        return prev1;
    }

    private static List<TestCase> GenerateCoinChangeTestCases(int count)
    {
        var coinSets = new[] { new[] { 1, 5, 10 }, new[] { 1, 2, 5 }, new[] { 2 }, new[] { 1, 3, 4 } };
        var cases = new List<TestCase>();

        for (int i = 0; i < count; i++)
        {
            var coins = coinSets[_random.Next(coinSets.Length)];
            int amount = _random.Next(1, 20);
            cases.Add(TestCase.Create(
                $"[{string.Join(",", coins)}],{amount}",
                CoinChange(coins, amount).ToString(),
                i >= 2));
        }

        return cases;
    }

    private static int CoinChange(int[] coins, int amount)
    {
        var dp = new int[amount + 1];
        Array.Fill(dp, amount + 1);
        dp[0] = 0;
        for (int i = 1; i <= amount; i++)
            foreach (var coin in coins)
                if (coin <= i) dp[i] = Math.Min(dp[i], dp[i - coin] + 1);
        return dp[amount] > amount ? -1 : dp[amount];
    }
}
