namespace Benchmark.Engine.Evaluation;

public class FailureAnalyzer
{
    /// <summary>
    /// Analises results. Returns a dictionary with error messages as keys and their occurrence counts as values.
    /// </summary>
    public Dictionary<string, int> AnalyzeFailures(List<EvaluationResult> results)
    {
        var failureCounts = new Dictionary<string, int>();

        foreach (var res in results.Where(r => !r.Passed))
        {
            string key = res.ErrorMessage;
            if (!failureCounts.ContainsKey(key))
                failureCounts[key] = 0;

            failureCounts[key]++;
        }

        return failureCounts;
    }
}
