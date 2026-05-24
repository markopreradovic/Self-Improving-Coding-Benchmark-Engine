using Benchmark.Domain.Problems;
using Benchmark.Domain.Problems.Common;
using ErrorOr;

namespace Benchmark.Application.Generators;

public class StringProblemGenerator : ITypedProblemGenerator
{
    private static readonly Random _random = new();

    public ProblemCategory SupportedCategory => ProblemCategory.String;

    private readonly List<(string Title, string Description, string FuncSig, Func<int, List<TestCase>> TestCaseGen)> _templates;

    public StringProblemGenerator()
    {
        _templates = new()
        {
            ("Reverse Words in a String Variant",
             "Given an input string s, reverse the order of the words. Words are separated by spaces. Return the result with no leading or trailing spaces.",
             "string ReverseWords(string s)",
             GenerateReverseWordsTestCases),

            ("Valid Palindrome Variant",
             "Given a string s, return true if it is a palindrome considering only alphanumeric characters and ignoring case, otherwise return false.",
             "bool IsPalindrome(string s)",
             GenerateValidPalindromeTestCases),

            ("Longest Substring Without Repeating Characters Variant",
             "Given a string s, find the length of the longest substring without repeating characters.",
             "int LengthOfLongestSubstring(string s)",
             GenerateLongestSubstringTestCases),
        };
    }

    public ErrorOr<CodingProblem> Generate(
        ProblemCategory category,
        DifficultyLevel? difficulty = null,
        int minTestCases = 3)
    {
        if (category != ProblemCategory.String)
            return DomainErrors.Problem.UnsupportedCategory;

        var diff = difficulty ?? (DifficultyLevel)_random.Next(0, 3);
        var template = _templates[_random.Next(_templates.Count)];
        var cases = template.TestCaseGen(minTestCases + _random.Next(0, 3));

        return CodingProblem.Create(
            title: template.Title + " #" + _random.Next(1000, 9999),
            description: template.Description,
            category: ProblemCategory.String,
            difficulty: diff,
            functionSignature: template.FuncSig,
            testCases: cases);
    }

    private static List<TestCase> GenerateReverseWordsTestCases(int count)
    {
        var wordBank = new[] { "the", "sky", "is", "blue", "hello", "world", "a", "good", "coding", "fun", "day" };
        var cases = new List<TestCase>();

        for (int i = 0; i < count; i++)
        {
            int wordCount = _random.Next(2, 6);
            var words = Enumerable.Range(0, wordCount)
                .Select(_ => wordBank[_random.Next(wordBank.Length)])
                .ToArray();
            string input = string.Join(" ", words);
            string expected = string.Join(" ", words.Reverse());
            cases.Add(TestCase.Create($"\"{input}\"", $"\"{expected}\"", i >= 2));
        }

        return cases;
    }

    private static List<TestCase> GenerateValidPalindromeTestCases(int count)
    {
        var samples = new[]
        {
            "A man, a plan, a canal: Panama",
            "race a car",
            "Was it a car or a cat I saw?",
            "No lemon, no melon",
            "hello",
            "Never odd or even",
            "not a palindrome",
        };
        var cases = new List<TestCase>();

        for (int i = 0; i < count; i++)
        {
            string s = samples[_random.Next(samples.Length)];
            var filtered = new string(s.Where(char.IsLetterOrDigit).Select(char.ToLower).ToArray());
            bool isPalin = filtered == new string(filtered.Reverse().ToArray());
            cases.Add(TestCase.Create($"\"{s}\"", isPalin.ToString().ToLower(), i >= 2));
        }

        return cases;
    }

    private static List<TestCase> GenerateLongestSubstringTestCases(int count)
    {
        var fixed_ = new[] { "abcabcbb", "bbbbb", "pwwkew", "dvdf", "tmmzuxt", "abcdef", "aab" };
        var cases = new List<TestCase>();

        for (int i = 0; i < count; i++)
        {
            string s = i < fixed_.Length
                ? fixed_[i]
                : GenerateRandomString(5, 10);
            int result = LongestSubstringLength(s);
            cases.Add(TestCase.Create($"\"{s}\"", result.ToString(), i >= 2));
        }

        return cases;
    }

    private static string GenerateRandomString(int minLen, int maxLen)
    {
        int len = _random.Next(minLen, maxLen);
        return new string(Enumerable.Range(0, len).Select(_ => (char)('a' + _random.Next(0, 8))).ToArray());
    }

    private static int LongestSubstringLength(string s)
    {
        var seen = new Dictionary<char, int>();
        int max = 0, start = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (seen.TryGetValue(s[i], out int prev) && prev >= start)
                start = prev + 1;
            seen[s[i]] = i;
            max = Math.Max(max, i - start + 1);
        }
        return max;
    }
}
