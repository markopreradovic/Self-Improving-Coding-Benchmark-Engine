using System.Diagnostics;
using Benchmark.Domain.Sandbox;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Benchmark.Sandbox.Execution;

public class CodeRunner : ICodeRunner
{
    // Serialised execution: Console.SetOut is process-global.
    // True isolation requires Docker (planned for future).
    private static readonly SemaphoreSlim _lock = new(1, 1);

    private static readonly ScriptOptions _scriptOptions = ScriptOptions.Default
        .AddReferences(
            typeof(object).Assembly,
            typeof(Enumerable).Assembly,
            typeof(System.Text.Json.JsonSerializer).Assembly)
        .AddImports(
            "System",
            "System.Collections.Generic",
            "System.Linq",
            "System.Text.Json");

    public async Task<ExecutionResult> RunAsync(
        string code,
        string? stdinInput = null,
        SandboxLimits? limits = null,
        CancellationToken ct = default)
    {
        limits ??= SandboxLimits.Default;

        await _lock.WaitAsync(ct);

        var originalOut = Console.Out;
        var originalIn = Console.In;
        var sw = new Stopwatch();

        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(limits.TimeLimit);

            var outputWriter = new StringWriter();
            Console.SetOut(outputWriter);
            if (stdinInput is not null)
                Console.SetIn(new StringReader(stdinInput));

            sw.Start();
            await CSharpScript.RunAsync(code, _scriptOptions, cancellationToken: cts.Token);
            sw.Stop();

            return new ExecutionResult(
                ExecutionStatus.Accepted,
                outputWriter.ToString().TrimEnd(),
                null,
                sw.Elapsed);
        }
        catch (CompilationErrorException ex)
        {
            sw.Stop();
            return new ExecutionResult(
                ExecutionStatus.CompileError,
                null,
                string.Join("\n", ex.Diagnostics.Select(d => d.ToString())),
                sw.Elapsed);
        }
        catch (OperationCanceledException)
        {
            sw.Stop();
            return new ExecutionResult(
                ExecutionStatus.TimeLimitExceeded,
                null,
                $"Execution exceeded time limit of {limits.TimeLimit.TotalSeconds}s.",
                sw.Elapsed);
        }
        catch (Exception ex)
        {
            sw.Stop();
            var message = ex.InnerException?.Message ?? ex.Message;
            return new ExecutionResult(ExecutionStatus.RuntimeError, null, message, sw.Elapsed);
        }
        finally
        {
            Console.SetOut(originalOut);
            Console.SetIn(originalIn);
            _lock.Release();
        }
    }
}
