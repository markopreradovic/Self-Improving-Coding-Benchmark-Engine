using Benchmark.Engine.Problems.Models;

namespace Benchmark.Engine.Problems
{
    /// <summary>
    /// Generates coding problems for benchmarking
    /// </summary>
    public class ProblemGenerator
    {
        private readonly Random _rand = new();

        /// <summary>
        /// Generate a batch of problems by category and difficulty
        /// </summary>
        /// <param name="category">Category e.g., Arrays, DP, Graphs</param>
        /// <param name="difficulty">Difficulty: Easy, Medium, Hard</param>
        /// <param name="count">Number of problems</param>
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
        /// Generates a single dummy problem based on category and difficulty
        /// </summary>
        private CodingProblem GenerateProblem(string category, string difficulty, int index)
        {
            switch (category.ToLower())
            {
                case "arrays":
                    return GenerateArrayProblem(difficulty, index);

                case "dp":
                    return GenerateDPProblem(difficulty, index);

                case "graphs":
                    return GenerateGraphProblem(difficulty, index);

                default:
                    // fallback simple sum problem
                    return new CodingProblem
                    {
                        Title = $"Sum Two Numbers #{index}",
                        ExpectedFunctionSignature = "int Sum(int a, int b)",
                        TestCases =
                        {
                            new() { Input = "2,3", ExpectedOutput = "5" },
                            new() { Input = "10,7", ExpectedOutput = "17" }
                        }
                    };
            }
        }

        private CodingProblem GenerateArrayProblem(string difficulty, int index)
        {
            return new CodingProblem
            {
                Title = $"Array Sum #{index} ({difficulty})",
                ExpectedFunctionSignature = "int SumArray(int[] arr)",
                TestCases =
                {
                    new() { Input = "[1,2,3]", ExpectedOutput = "6" },
                    new() { Input = "[4,5,6]", ExpectedOutput = "15" }
                }
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
    }
}
