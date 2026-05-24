using Benchmark.Domain.Problems;
using Benchmark.Domain.Problems.Common;
using ErrorOr;

namespace Benchmark.Application.Generators;

public class TreeProblemGenerator : ITypedProblemGenerator
{
    private static readonly Random _random = new();

    public ProblemCategory SupportedCategory => ProblemCategory.Tree;

    private readonly List<(string Title, string Description, string FuncSig, Func<int, List<TestCase>> TestCaseGen)> _templates;

    public TreeProblemGenerator()
    {
        _templates = new()
        {
            ("Maximum Depth of Binary Tree Variant",
             "Given the root of a complete binary tree represented as a level-order array, return its maximum depth.",
             "int MaxDepth(int[] tree)",
             GenerateMaxDepthTestCases),

            ("Sum of All Tree Nodes Variant",
             "Given a complete binary tree represented as a level-order array, return the sum of all node values.",
             "int SumTree(int[] tree)",
             GenerateSumTreeTestCases),

            ("Count Leaf Nodes Variant",
             "Given a complete binary tree represented as a level-order array, return the number of leaf nodes.",
             "int CountLeaves(int[] tree)",
             GenerateCountLeavesTestCases),
        };
    }

    public ErrorOr<CodingProblem> Generate(
        ProblemCategory category,
        DifficultyLevel? difficulty = null,
        int minTestCases = 3)
    {
        if (category != ProblemCategory.Tree)
            return DomainErrors.Problem.UnsupportedCategory;

        var diff = difficulty ?? (DifficultyLevel)_random.Next(0, 3);
        var template = _templates[_random.Next(_templates.Count)];
        var cases = template.TestCaseGen(minTestCases + _random.Next(0, 3));

        return CodingProblem.Create(
            title: template.Title + " #" + _random.Next(1000, 9999),
            description: template.Description,
            category: ProblemCategory.Tree,
            difficulty: diff,
            functionSignature: template.FuncSig,
            testCases: cases);
    }

    // Returns a complete binary tree (2^depth - 1 nodes) as a level-order array.
    private static int[] GenerateCompleteTree(int depth)
    {
        int nodeCount = (1 << depth) - 1;
        return Enumerable.Range(0, nodeCount).Select(_ => _random.Next(1, 100)).ToArray();
    }

    private static List<TestCase> GenerateMaxDepthTestCases(int count)
    {
        var cases = new List<TestCase>();
        for (int i = 0; i < count; i++)
        {
            int depth = _random.Next(1, 5);
            var tree = GenerateCompleteTree(depth);
            cases.Add(TestCase.Create(
                $"[{string.Join(",", tree)}]",
                depth.ToString(),
                i >= 2));
        }
        return cases;
    }

    private static List<TestCase> GenerateSumTreeTestCases(int count)
    {
        var cases = new List<TestCase>();
        for (int i = 0; i < count; i++)
        {
            int depth = _random.Next(1, 4);
            var tree = GenerateCompleteTree(depth);
            cases.Add(TestCase.Create(
                $"[{string.Join(",", tree)}]",
                tree.Sum().ToString(),
                i >= 2));
        }
        return cases;
    }

    private static List<TestCase> GenerateCountLeavesTestCases(int count)
    {
        var cases = new List<TestCase>();
        for (int i = 0; i < count; i++)
        {
            int depth = _random.Next(1, 4);
            var tree = GenerateCompleteTree(depth);
            int leaves = (tree.Length + 1) / 2; // holds for perfect binary trees
            cases.Add(TestCase.Create(
                $"[{string.Join(",", tree)}]",
                leaves.ToString(),
                i >= 2));
        }
        return cases;
    }
}
