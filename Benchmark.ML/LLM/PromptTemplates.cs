using Benchmark.Domain.Problems;

namespace Benchmark.ML.LLM;

public static class PromptTemplates
{
    public static string BuildSolvingPrompt(CodingProblem problem)
    {
        var sample = problem.TestCases.FirstOrDefault(t => !t.IsHidden)
                     ?? problem.TestCases.First();

        return $"""
            You are an expert C# programmer. Solve the following {problem.Difficulty} {problem.Category} coding problem.

            ## Problem: {problem.Title}

            {problem.Description}

            ## Function Signature
            {problem.FunctionSignature}

            ## Sample Test Case
            Input:  {sample.Input}
            Output: {sample.ExpectedOutput}

            ## Instructions
            Write a complete, executable C# top-level script that:
            1. Reads input from Console.ReadLine()
            2. Parses it, calls the solution function, and prints the result with Console.WriteLine()
            3. Uses System.Text.Json for any JSON serialization or deserialization

            Rules:
            - Return ONLY the C# code inside a ```csharp code block, nothing else
            - No class wrapper — use top-level statements
            - The output must exactly match the expected format shown above

            ```csharp
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text.Json;

            // implement here
            ```
            """;
    }

    public static string BuildGenerationPrompt(
        ProblemCategory category,
        DifficultyLevel difficulty,
        int minTestCases)
    {
        return $$"""
            Generate a {{difficulty}} {{category}} coding problem in LeetCode style.

            Return ONLY valid JSON with NO extra text or markdown — not even a code fence. Use exactly this structure:
            {
              "title": "Problem Title",
              "description": "Full description with examples and constraints",
              "functionSignature": "ReturnType MethodName(ParamType param)",
              "testCases": [
                {"input": "...", "expectedOutput": "...", "isHidden": false},
                {"input": "...", "expectedOutput": "...", "isHidden": true}
              ]
            }

            Requirements:
            - Minimum {{minTestCases}} test cases; at least 1 must be hidden (isHidden: true)
            - Function signature in C# syntax
            - All test cases must be correct and deterministic
            """;
    }
}
