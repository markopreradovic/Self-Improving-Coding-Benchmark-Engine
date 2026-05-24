using System.Text;
using System.Text.Json;
using Benchmark.Domain.LLM;
using Microsoft.Extensions.Options;

namespace Benchmark.ML.LLM;

public class AnthropicLlmClient : ILlmClient
{
    private static readonly JsonSerializerOptions _snakeCase = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    private readonly HttpClient _http;
    private readonly AnthropicOptions _options;

    public string ModelName => _options.Model;

    public AnthropicLlmClient(HttpClient http, IOptions<AnthropicOptions> options)
    {
        _options = options.Value;
        _http = http;
        _http.BaseAddress = new Uri("https://api.anthropic.com");
        _http.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", _options.ApiKey);
        _http.DefaultRequestHeaders.TryAddWithoutValidation("anthropic-version", "2023-06-01");
    }

    public async Task<string> CompleteAsync(
        string prompt,
        int maxTokens = 4096,
        CancellationToken ct = default)
    {
        var body = new
        {
            Model = _options.Model,
            MaxTokens = maxTokens,
            Messages = new[] { new { Role = "user", Content = prompt } }
        };

        var json = JsonSerializer.Serialize(body, _snakeCase);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.PostAsync("/v1/messages", content, ct);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        using var doc = JsonDocument.Parse(responseJson);

        return doc.RootElement
            .GetProperty("content")[0]
            .GetProperty("text")
            .GetString()
            ?? throw new InvalidOperationException("Anthropic API returned an empty response.");
    }
}
