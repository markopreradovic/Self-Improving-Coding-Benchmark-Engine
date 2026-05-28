using System.Net.Http.Json;
using Benchmark.Application.Common.DTOs;

namespace Benchmark.UI.Services;

public class BenchmarkApiClient
{
    private readonly HttpClient _http;

    public BenchmarkApiClient(HttpClient http) => _http = http;

    public Task<List<ProblemSummaryDto>?> GetProblemsAsync(
        string? category = null, string? difficulty = null, int page = 1, int pageSize = 20)
    {
        var url = $"/api/problems?page={page}&pageSize={pageSize}";
        if (category   is not null) url += $"&category={category}";
        if (difficulty is not null) url += $"&difficulty={difficulty}";
        return _http.GetFromJsonAsync<List<ProblemSummaryDto>>(url);
    }

    public Task<MetricsSummaryDto?> GetMetricsSummaryAsync()
        => _http.GetFromJsonAsync<MetricsSummaryDto>("/api/metrics/summary");

    public Task<List<EvaluationResultDto>?> GetEvaluationsAsync(Guid problemId)
        => _http.GetFromJsonAsync<List<EvaluationResultDto>>($"/api/evaluation/{problemId}");

    public Task<List<ModelVersionDto>?> GetModelVersionsAsync()
        => _http.GetFromJsonAsync<List<ModelVersionDto>>("/api/training/versions");
}
