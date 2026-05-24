using System.Text.Json;
using System.Text.RegularExpressions;
using Benchmark.Domain.LLM;
using Benchmark.Domain.Problems;
using Benchmark.Domain.Problems.Common;
using ErrorOr;

namespace Benchmark.Application.Generators;

public class LlmProblemGenerator : IAsyncProblemGenerator
{
    private readonly ILlmClient _llm;

    public LlmProblemGenerator(ILlmClient llm) => _llm = llm;

    public async Task<ErrorOr<CodingProblem>> GenerateAsync(
        ProblemCategory category,
        DifficultyLevel? difficulty = null,
        int minTestCases = 3,
        CancellationToken ct = default)
    {
        var diff = difficulty ?? (DifficultyLevel)Random.Shared.Next(0, 3);

        // Build prompt — PromptTemplates lives in ML but the text is inlined here
        // to keep Domain free of ML dependencies.
        var prompt = BuildPrompt(category, diff, minTestCases);

        string raw;
        try
        {
            raw = await _llm.CompleteAsync(prompt, maxTokens: 2048, ct: ct);
        }
        catch (Exception ex)
        {
            return Error.Failure("LlmGenerator.ApiError", ex.Message);
        }

        return ParseResponse(raw, category, diff);
    }

    private static string BuildPrompt(ProblemCategory category, DifficultyLevel difficulty, int minTestCases) =>
        $$"""
         Generate a {{difficulty}} {{category}} coding problem in LeetCode style.

         Return ONLY valid JSON — no markdown, no code fences. Exact structure:
         {
           "title": "Problem Title",
           "description": "Full description with examples and constraints",
           "functionSignature": "ReturnType MethodName(ParamType param)",
           "testCases": [
             {"input": "...", "expectedOutput": "...", "isHidden": false},
             {"input": "...", "expectedOutput": "...", "isHidden": true}
           ]
         }

         Requirements: minimum {{minTestCases}} test cases, at least 1 hidden.
         C# function signature syntax. All test cases must be correct and deterministic.
         """;

    private static ErrorOr<CodingProblem> ParseResponse(
        string raw, ProblemCategory category, DifficultyLevel difficulty)
    {
        try
        {
            var json = ExtractJson(raw);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var title = root.GetProperty("title").GetString() ?? string.Empty;
            var description = root.GetProperty("description").GetString() ?? string.Empty;
            var funcSig = root.GetProperty("functionSignature").GetString() ?? string.Empty;

            var testCases = root.GetProperty("testCases")
                .EnumerateArray()
                .Select(tc => TestCase.Create(
                    tc.GetProperty("input").GetString() ?? string.Empty,
                    tc.GetProperty("expectedOutput").GetString() ?? string.Empty,
                    tc.TryGetProperty("isHidden", out var h) && h.GetBoolean(),
                    tc.TryGetProperty("explanation", out var e) ? e.GetString() : null))
                .ToList();

            return CodingProblem.Create(title, description, category, difficulty, funcSig, testCases);
        }
        catch (Exception ex)
        {
            return Error.Failure("LlmGenerator.ParseError", $"Failed to parse LLM response: {ex.Message}");
        }
    }

    private static string ExtractJson(string response)
    {
        var match = Regex.Match(response, @"```(?:json)?\s*([\s\S]*?)```");
        if (match.Success) return match.Groups[1].Value.Trim();

        var start = response.IndexOf('{');
        var end = response.LastIndexOf('}');
        if (start >= 0 && end > start) return response[start..(end + 1)];

        return response;
    }
}
