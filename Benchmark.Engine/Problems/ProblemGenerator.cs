using Benchmark.Engine.Problems.Models;

namespace Benchmark.Engine.Problems
{
    public class ProblemGenerator
    {
        private readonly Random _rand = new();

        /// <summary>
        /// Generate a batch of problems by category and difficulty
        /// </summary>
        public async Task<List<CodingProblem>> GenerateBatchAsync(
            string category,
            string difficulty,
            int count)
        {
            var problems = new List<CodingProblem>();

            for (int i = 0; i < count; i++)
            {
                var problem = GenerateProblem(category, difficulty, i);
                problems.Add(problem);
            }

            return problems;
        }

        /// <summary>
        /// Generate a single problem based on category and difficulty
        /// </summary>
        private CodingProblem GenerateProblem(string category, string difficulty, int index)
        {
            switch (category.ToLower())
            {
                case "arrays": return GenerateArrayProblem(difficulty, index);
                case "dp": return GenerateDPProblem(difficulty, index);
                case "graphs": return GenerateGraphProblem(difficulty, index);
                case "strings": return GenerateStringProblem(difficulty, index);
                case "trees": return GenerateTreeProblem(difficulty, index);
                case "backtracking": return GenerateBacktrackingProblem(difficulty, index);
                case "sorting": return GenerateSortingProblem(difficulty, index);
                default:
                    return new CodingProblem
                    {
                        Title = $"Fallback Problem #{index}",
                        ExpectedFunctionSignature = "int Sum(int a, int b)",
                        TestCases =
                        {
                            new() { Input = "2,3", ExpectedOutput = "5" },
                            new() { Input = "10,7", ExpectedOutput = "17" }
                        }
                    };
            }
        }

        #region Category Generators

        private CodingProblem GenerateArrayProblem(string difficulty, int index)
        {
            int size = difficulty switch
            {
                "Easy" => 5,
                "Medium" => 10,
                "Hard" => 20,
                _ => 5
            };

            var testCases = Enumerable.Range(1, 3)
                .Select(_ => new TestCase
                {
                    Input = $"[{string.Join(",", Enumerable.Range(1, size).Select(_ => _rand.Next(1, 10)))}]",
                    ExpectedOutput = "calculated" // placeholder
                }).ToList();

            return new CodingProblem
            {
                Title = $"Array Sum #{index} ({difficulty})",
                ExpectedFunctionSignature = "int SumArray(int[] arr)",
                TestCases = testCases
            };
        }

        private CodingProblem GenerateDPProblem(string difficulty, int index)
        {
            return new CodingProblem
            {
                Title = $"Fibonacci #{index} ({difficulty})",
                ExpectedFunctionSignature = "int Fib(int n)",
                TestCases =
                {
                    new() { Input = "5", ExpectedOutput = "5" },
                    new() { Input = "7", ExpectedOutput = "13" }
                }
            };
        }

        private CodingProblem GenerateGraphProblem(string difficulty, int index)
        {
            return new CodingProblem
            {
                Title = $"Graph BFS #{index} ({difficulty})",
                ExpectedFunctionSignature = "List<int> BFS(int start, int[][] edges)",
                TestCases =
                {
                    new() { Input = "0, [[0,1],[0,2],[1,2]]", ExpectedOutput = "[0,1,2]" },
                    new() { Input = "1, [[0,1],[1,2]]", ExpectedOutput = "[1,0,2]" }
                }
            };
        }

        private CodingProblem GenerateStringProblem(string difficulty, int index)
        {
            return new CodingProblem
            {
                Title = $"Palindrome Check #{index} ({difficulty})",
                ExpectedFunctionSignature = "bool IsPalindrome(string s)",
                TestCases =
                {
                    new() { Input = "\"racecar\"", ExpectedOutput = "true" },
                    new() { Input = "\"hello\"", ExpectedOutput = "false" }
                }
            };
        }

        private CodingProblem GenerateTreeProblem(string difficulty, int index)
        {
            return new CodingProblem
            {
                Title = $"Tree Max Depth #{index} ({difficulty})",
                ExpectedFunctionSignature = "int MaxDepth(TreeNode root)",
                TestCases =
                {
                    new() { Input = "Tree1", ExpectedOutput = "3" },
                    new() { Input = "Tree2", ExpectedOutput = "4" }
                }
            };
        }

        private CodingProblem GenerateBacktrackingProblem(string difficulty, int index)
        {
            return new CodingProblem
            {
                Title = $"N-Queens #{index} ({difficulty})",
                ExpectedFunctionSignature = "List<List<string>> SolveNQueens(int n)",
                TestCases =
                {
                    new() { Input = "4", ExpectedOutput = "[[\".Q..\",\"...Q\",\"Q...\",\"..Q.\"]]" }
                }
            };
        }

        private CodingProblem GenerateSortingProblem(string difficulty, int index)
        {
            return new CodingProblem
            {
                Title = $"Sorting Array #{index} ({difficulty})",
                ExpectedFunctionSignature = "int[] SortArray(int[] arr)",
                TestCases =
                {
                    new() { Input = "[5,3,1,4,2]", ExpectedOutput = "[1,2,3,4,5]" }
                }
            };
        }

        #endregion
    }
}
