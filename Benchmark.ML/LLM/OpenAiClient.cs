namespace Benchmark.ML.LLM;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class OpenAiClient : ILlmClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    public OpenAiClient(string apiKey, string model = "gpt-3.5-turbo")
    {
        _apiKey = apiKey;
        _model = model;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.openai.com/v1/") 
        };
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Benchmark.Worker");
    }

    public async Task<string> GenerateCodeAsync(string prompt)
    {
        var requestBody = new
        {
            model = _model,
            messages = new[]
            {
                new { role = "system", content = "You are a helpful coding assistant. Respond only with the code solution, no explanations." },
                new { role = "user", content = prompt }
            },
            temperature = 0.0,
            max_tokens = 1000
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        try
        {
            var response = await _httpClient.PostAsync("chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"OpenAI API Error: {response.StatusCode} - {errorContent}");
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            using var json = await JsonDocument.ParseAsync(responseStream);

            var code = json.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return code?.Trim() ?? string.Empty;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to generate code: {ex.Message}", ex);
        }
    }

    public async Task<string> FineTuneAsync(string datasetPath, CancellationToken cancellationToken = default)
    {
        await Task.Delay(1000, cancellationToken);
        return "fine-tuned-model-id";
    }
}