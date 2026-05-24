using Benchmark.Application.Generators;
using Benchmark.Domain.LLM;
using Benchmark.ML.LLM;
using Benchmark.ML.Solvers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Benchmark.ML;

public static class DependencyInjection
{
    public static IServiceCollection AddMlServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AnthropicOptions>(configuration.GetSection("LLM:Anthropic"));
        services.AddHttpClient<ILlmClient, AnthropicLlmClient>();
        services.AddScoped<ISolver, LlmSolver>();
        services.AddScoped<IAsyncProblemGenerator, LlmProblemGenerator>();

        return services;
    }
}
