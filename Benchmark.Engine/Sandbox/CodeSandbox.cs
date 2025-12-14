using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Benchmark.Engine.Problems.Models;

namespace Benchmark.Engine.Sandbox;

public class CodeSandbox
{
    private readonly ScriptOptions _options;

    public CodeSandbox()
    {
        _options = ScriptOptions.Default
            .AddReferences(typeof(object).Assembly)
            .AddImports("System");
    }

    public async Task<SandboxResult> ExecuteAsync(
        CodingProblem problem,
        string solutionCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Creating script
            string script = BuildScript(problem, solutionCode);

            // Code execution
            var result = await CSharpScript.EvaluateAsync<object>(
                script,
                _options,
                cancellationToken: cancellationToken);

            return new SandboxResult
            {
                Success = true,
                Output = result?.ToString() ?? string.Empty
            };
        }
        catch (CompilationErrorException ex)
        {
            return new SandboxResult
            {
                Success = false,
                Error = string.Join("\n", ex.Diagnostics)
            };
        }
        catch (Exception ex)
        {
            return new SandboxResult
            {
                Success = false,
                Error = ex.Message
            };
        }
    }

    private string BuildScript(CodingProblem problem, string solutionCode)
    {
        var test = problem.TestCases.First();

        return $@"
{solutionCode}

return {ExtractMethodName(problem.ExpectedFunctionSignature)}({test.Input});
";
    }

    private string ExtractMethodName(string signature)
    {
        // ex: "int Sum(int a, int b)" → "Sum"
        var parts = signature.Split(' ');
        return parts[1].Split('(')[0];
    }
}
