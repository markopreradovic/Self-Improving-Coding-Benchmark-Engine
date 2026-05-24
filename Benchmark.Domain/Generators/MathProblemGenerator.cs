using Benchmark.Domain.Problems;
using Benchmark.Domain.Problems.Common;
using ErrorOr;

namespace Benchmark.Application.Generators;

public class MathProblemGenerator : ITypedProblemGenerator
{
    private static readonly Random _random = new();

    public ProblemCategory SupportedCategory => ProblemCategory.Math;

    private readonly List<(string Title, string Description, string FuncSig, Func<int, List<TestCase>> TestCaseGen)> _templates;

    public MathProblemGenerator()
    {
        _templates = new()
        {
            ("Count Primes Variant",
             "Given an integer n, return the number of prime numbers that are strictly less than n.",
             "int CountPrimes(int n)",
             GenerateCountPrimesTestCases),

            ("Power of Two Variant",
             "Given an integer n, return true if it is a power of two, otherwise return false.",
             "bool IsPowerOfTwo(int n)",
             GeneratePowerOfTwoTestCases),

            ("Reverse Integer Variant",
             "Given a signed 32-bit integer x, return x with its digits reversed. If reversing causes the value to go outside the signed 32-bit integer range, return 0.",
             "int Reverse(int x)",
             GenerateReverseIntTestCases),
        };
    }

    public ErrorOr<CodingProblem> Generate(
        ProblemCategory category,
        DifficultyLevel? difficulty = null,
        int minTestCases = 3)
    {
        if (category != ProblemCategory.Math)
            return DomainErrors.Problem.UnsupportedCategory;

        var diff = difficulty ?? (DifficultyLevel)_random.Next(0, 3);
        var template = _templates[_random.Next(_templates.Count)];
        var cases = template.TestCaseGen(minTestCases + _random.Next(0, 3));

        return CodingProblem.Create(
            title: template.Title + " #" + _random.Next(1000, 9999),
            description: template.Description,
            category: ProblemCategory.Math,
            difficulty: diff,
            functionSignature: template.FuncSig,
            testCases: cases);
    }

    private static List<TestCase> GenerateCountPrimesTestCases(int count)
    {
        var cases = new List<TestCase>();
        for (int i = 0; i < count; i++)
        {
            int n = _random.Next(2, 50);
            cases.Add(TestCase.Create(n.ToString(), CountPrimes(n).ToString(), i >= 2));
        }
        return cases;
    }

    private static int CountPrimes(int n)
    {
        if (n < 2) return 0;
        var sieve = new bool[n];
        Array.Fill(sieve, true);
        sieve[0] = sieve[1] = false;
        for (int i = 2; i * i < n; i++)
            if (sieve[i])
                for (int j = i * i; j < n; j += i)
                    sieve[j] = false;
        return sieve.Count(x => x);
    }

    private static List<TestCase> GeneratePowerOfTwoTestCases(int count)
    {
        var powersOfTwo = new[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024 };
        var cases = new List<TestCase>();

        for (int i = 0; i < count; i++)
        {
            int n;
            if (_random.NextDouble() > 0.4)
                n = powersOfTwo[_random.Next(powersOfTwo.Length)];
            else
                n = _random.Next(1, 200);

            bool isPow = n > 0 && (n & (n - 1)) == 0;
            cases.Add(TestCase.Create(n.ToString(), isPow.ToString().ToLower(), i >= 2));
        }

        return cases;
    }

    private static List<TestCase> GenerateReverseIntTestCases(int count)
    {
        var cases = new List<TestCase>();
        for (int i = 0; i < count; i++)
        {
            int x = _random.Next(-9999, 9999);
            cases.Add(TestCase.Create(x.ToString(), ReverseInt(x).ToString(), i >= 2));
        }
        return cases;
    }

    private static int ReverseInt(int x)
    {
        long result = 0;
        while (x != 0)
        {
            result = result * 10 + x % 10;
            x /= 10;
        }
        return result is > int.MaxValue or < int.MinValue ? 0 : (int)result;
    }
}
