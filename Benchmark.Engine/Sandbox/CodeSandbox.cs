using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Benchmark.Engine.Problems.Models;
using System.Diagnostics;

namespace Benchmark.Engine.Sandbox;

public class CodeSandbox
{
    private readonly ScriptOptions _options;

    public CodeSandbox()
    {
        _options = ScriptOptions.Default
            .WithEmitDebugInformation(false)
            .AddReferences(typeof(object).Assembly)
            .AddImports("System");
    }

    public async Task<SandboxResult> ExecuteAsync(
        CodingProblem problem,
        string solutionCode,
        CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            // 1️⃣ Limit dužine koda
            if (solutionCode.Length > 10_000)
            {
                return new SandboxResult
                {
                    Success = false,
                    Error = "Solution code too large",
                    Metrics = new ExecutionMetrics { TimedOut = false, ExecutionTime = sw.Elapsed }
                };
            }

            // 2️⃣ Heuristika za infinite loop
            if (solutionCode.Contains("while(true)") || solutionCode.Contains("for(;;)"))
            {
                return new SandboxResult
                {
                    Success = false,
                    Error = "Potential infinite loop detected",
                    Metrics = new ExecutionMetrics { TimedOut = false, ExecutionTime = sw.Elapsed }
                };
            }

            // 3️⃣ Kreiramo linked cancellation token sa timeoutom
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(2)); // 2 sekunde timeout

            // 4️⃣ Build C# script za izvršavanje funkcije
            string script = BuildScript(problem, solutionCode);

            // 5️⃣ Execute kod
            var result = await CSharpScript.EvaluateAsync<object>(
                script,
                _options,
                cancellationToken: cts.Token);

            sw.Stop();
            return new SandboxResult
            {
                Success = true,
                Output = result?.ToString() ?? string.Empty,
                Metrics = new ExecutionMetrics { TimedOut = false, ExecutionTime = sw.Elapsed }
            };
        }
        catch (OperationCanceledException)
        {
            sw.Stop();
            return new SandboxResult
            {
                Success = false,
                Error = "Execution timed out",
                Metrics = new ExecutionMetrics { TimedOut = true, ExecutionTime = sw.Elapsed }
            };
        }
        catch (CompilationErrorException ex)
        {
            sw.Stop();
            return new SandboxResult
            {
                Success = false,
                Error = string.Join("\n", ex.Diagnostics),
                Metrics = new ExecutionMetrics { TimedOut = false, ExecutionTime = sw.Elapsed }
            };
        }
        catch (Exception ex)
        {
            sw.Stop();
            return new SandboxResult
            {
                Success = false,
                Error = ex.Message,
                Metrics = new ExecutionMetrics { TimedOut = false, ExecutionTime = sw.Elapsed }
            };
        }
    }

    // -------------------------
    // Privatne pomoćne funkcije
    // -------------------------

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
        // npr: "int Sum(int a, int b)" -> "Sum"
        var parts = signature.Split(' ');
        return parts[1].Split('(')[0];
    }
}
