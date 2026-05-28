using Benchmark.Application.Generators;
using Benchmark.Domain.LLM;
using Benchmark.Domain.Training;
using Benchmark.ML.Inference;
using Benchmark.ML.LLM;
using Benchmark.ML.Solvers;
using Benchmark.ML.Training;
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
        services.Configure<TrainingConfig>(configuration.GetSection(TrainingConfig.Section));

        services.AddHttpClient<ILlmClient, AnthropicLlmClient>();
        services.AddScoped<ISolver, LlmSolver>();
        services.AddScoped<IAsyncProblemGenerator, LlmProblemGenerator>();

        services.AddSingleton<OnnxModelRunner>();
        services.AddScoped<PythonBridge>();
        services.AddScoped<ITrainingOrchestrator, TrainingOrchestrator>();

        return services;
    }
}
