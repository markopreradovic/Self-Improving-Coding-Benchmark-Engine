using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Benchmark.ML.LLM;

public class OpenAiClient : ILlmClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenAiClient(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> GenerateCodeAsync(string prompt)
    {
        var requestBody = new
        {
            model = "gpt-4",
            messages = new[]
            {
                new { role = "system", content = "You are a C# coding assistant." },
                new { role = "user", content = prompt }
            },
            temperature = 0.2
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var code = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return code ?? string.Empty;
    }

    public async Task<string> FineTuneAsync(string datasetPath, CancellationToken cancellationToken = default)
    {
        var datasetJson = await File.ReadAllTextAsync(datasetPath, cancellationToken);

        var requestBody = new
        {
            training_file = datasetPath,
            model = "base-model"
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/fine-tunes", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(json);
        var fineTunedModelId = doc.RootElement.GetProperty("id").GetString();

        return fineTunedModelId ?? string.Empty;
    }
}
