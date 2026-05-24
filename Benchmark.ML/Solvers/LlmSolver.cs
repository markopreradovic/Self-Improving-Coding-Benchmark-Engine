using System.Diagnostics;
using System.Text.RegularExpressions;
using Benchmark.Domain.LLM;
using Benchmark.Domain.Problems;
using Benchmark.ML.LLM;

namespace Benchmark.ML.Solvers;

public class LlmSolver : ISolver
{
    private readonly ILlmClient _llm;

    public LlmSolver(ILlmClient llm) => _llm = llm;

    public async Task<SolveAttempt> SolveAsync(CodingProblem problem, CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var prompt = PromptTemplates.BuildSolvingPrompt(problem);
            var response = await _llm.CompleteAsync(prompt, ct: ct);
            sw.Stop();

            var code = ExtractCode(response);
            if (code is null)
                return new SolveAttempt(problem.Id, SolveStatus.ParseFailed, null,
                    "Could not extract C# code from LLM response.", sw.Elapsed, _llm.ModelName);

            return new SolveAttempt(problem.Id, SolveStatus.Generated, code, null, sw.Elapsed, _llm.ModelName);
        }
        catch (Exception ex)
        {
            sw.Stop();
            return new SolveAttempt(problem.Id, SolveStatus.LlmError, null, ex.Message, sw.Elapsed, _llm.ModelName);
        }
    }

    private static string? ExtractCode(string response)
    {
        var match = Regex.Match(response, @"```(?:csharp|cs)?\s*([\s\S]*?)```", RegexOptions.Multiline);
        if (match.Success) return match.Groups[1].Value.Trim();

        if (response.Contains("Console.") || response.Contains("using "))
            return response.Trim();

        return null;
    }
}
