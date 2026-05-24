using Benchmark.Domain.Problems;
using Benchmark.Domain.Problems.Common;
using ErrorOr;

namespace Benchmark.Application.Generators;

public class GraphProblemGenerator : ITypedProblemGenerator
{
    private static readonly Random _random = new();

    public ProblemCategory SupportedCategory => ProblemCategory.Graph;

    private readonly List<(string Title, string Description, string FuncSig, Func<int, List<TestCase>> TestCaseGen)> _templates;

    public GraphProblemGenerator()
    {
        _templates = new()
        {
            ("Number of Provinces Variant",
             "There are n cities. isConnected[i][j] = 1 if city i and city j are directly connected. Return the total number of provinces (groups of directly or indirectly connected cities).",
             "int FindCircleNum(int[][] isConnected)",
             GenerateProvincesTestCases),

            ("Number of Islands Variant",
             "Given an m x n 2D binary grid of '1's (land) and '0's (water), return the number of islands. An island is surrounded by water and is formed by connecting adjacent lands horizontally or vertically.",
             "int NumIslands(char[][] grid)",
             GenerateIslandsTestCases),

            ("Find Center of Star Graph Variant",
             "There is an undirected star graph consisting of n nodes. You are given a 2D integer array edges where edges[i] = [u, v]. Return the center of the given star graph.",
             "int FindCenter(int[][] edges)",
             GenerateCenterTestCases),
        };
    }

    public ErrorOr<CodingProblem> Generate(
        ProblemCategory category,
        DifficultyLevel? difficulty = null,
        int minTestCases = 3)
    {
        if (category != ProblemCategory.Graph)
            return DomainErrors.Problem.UnsupportedCategory;

        var diff = difficulty ?? (DifficultyLevel)_random.Next(0, 3);
        var template = _templates[_random.Next(_templates.Count)];
        var cases = template.TestCaseGen(minTestCases + _random.Next(0, 3));

        return CodingProblem.Create(
            title: template.Title + " #" + _random.Next(1000, 9999),
            description: template.Description,
            category: ProblemCategory.Graph,
            difficulty: diff,
            functionSignature: template.FuncSig,
            testCases: cases);
    }

    private static List<TestCase> GenerateProvincesTestCases(int count)
    {
        var cases = new List<TestCase>();

        for (int i = 0; i < count; i++)
        {
            int n = _random.Next(3, 6);
            var adj = new int[n, n];
            for (int r = 0; r < n; r++) adj[r, r] = 1;

            for (int r = 0; r < n; r++)
                for (int c = r + 1; c < n; c++)
                    if (_random.NextDouble() > 0.5)
                    { adj[r, c] = 1; adj[c, r] = 1; }

            int provinces = CountConnectedComponents(adj, n);

            var rows = Enumerable.Range(0, n)
                .Select(r => $"[{string.Join(",", Enumerable.Range(0, n).Select(c => adj[r, c]))}]");
            cases.Add(TestCase.Create($"[{string.Join(",", rows)}]", provinces.ToString(), i >= 2));
        }

        return cases;
    }

    private static int CountConnectedComponents(int[,] adj, int n)
    {
        var visited = new bool[n];
        int count = 0;
        for (int i = 0; i < n; i++)
        {
            if (!visited[i])
            {
                DfsGraph(adj, visited, i, n);
                count++;
            }
        }
        return count;
    }

    private static void DfsGraph(int[,] adj, bool[] visited, int node, int n)
    {
        visited[node] = true;
        for (int j = 0; j < n; j++)
            if (adj[node, j] == 1 && !visited[j])
                DfsGraph(adj, visited, j, n);
    }

    private static List<TestCase> GenerateIslandsTestCases(int count)
    {
        var cases = new List<TestCase>();

        for (int i = 0; i < count; i++)
        {
            int rows = _random.Next(3, 5), cols = _random.Next(3, 5);
            var grid = new char[rows, cols];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    grid[r, c] = _random.NextDouble() > 0.45 ? '1' : '0';

            int islands = CountIslands(grid, rows, cols);

            var rowStrs = Enumerable.Range(0, rows)
                .Select(r => $"[{string.Join(",", Enumerable.Range(0, cols).Select(c => $"\"{grid[r, c]}\""))}]");
            cases.Add(TestCase.Create($"[{string.Join(",", rowStrs)}]", islands.ToString(), i >= 2));
        }

        return cases;
    }

    private static int CountIslands(char[,] grid, int rows, int cols)
    {
        var visited = new bool[rows, cols];
        int count = 0;
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                if (grid[r, c] == '1' && !visited[r, c])
                {
                    BfsIsland(grid, visited, r, c, rows, cols);
                    count++;
                }
        return count;
    }

    private static void BfsIsland(char[,] grid, bool[,] visited, int startR, int startC, int rows, int cols)
    {
        var queue = new Queue<(int, int)>();
        queue.Enqueue((startR, startC));
        visited[startR, startC] = true;

        int[] dr = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, -1, 1 };

        while (queue.Count > 0)
        {
            var (r, c) = queue.Dequeue();
            for (int d = 0; d < 4; d++)
            {
                int nr = r + dr[d], nc = c + dc[d];
                if (nr >= 0 && nr < rows && nc >= 0 && nc < cols
                    && grid[nr, nc] == '1' && !visited[nr, nc])
                {
                    visited[nr, nc] = true;
                    queue.Enqueue((nr, nc));
                }
            }
        }
    }

    private static List<TestCase> GenerateCenterTestCases(int count)
    {
        var cases = new List<TestCase>();

        for (int i = 0; i < count; i++)
        {
            int n = _random.Next(4, 7);
            int center = _random.Next(1, n + 1);
            var others = Enumerable.Range(1, n).Where(x => x != center).ToList();

            var edges = others.Select(node =>
                _random.NextDouble() > 0.5
                    ? $"[{center},{node}]"
                    : $"[{node},{center}]");

            cases.Add(TestCase.Create($"[{string.Join(",", edges)}]", center.ToString(), i >= 2));
        }

        return cases;
    }
}
