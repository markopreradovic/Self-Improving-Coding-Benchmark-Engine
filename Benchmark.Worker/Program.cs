using Benchmark.Engine.Dataset;
using Benchmark.Engine.Evaluation;
using Benchmark.Engine.Runner;
using Benchmark.Engine.Sandbox;
using Benchmark.ML.LLM;
using Benchmark.ML.Training;
using Benchmark.Worker;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<FailureAnalyzer>();
builder.Services.AddSingleton<DatasetBuilder>();
builder.Services.AddSingleton<BenchmarkRunner>();
builder.Services.AddSingleton<ILlmClient>(sp => new OpenAiClient("api key"));
builder.Services.AddSingleton<TrainingOrchestrator>();
builder.Services.AddSingleton<CodeEvaluator>();
builder.Services.AddSingleton<CodeSandbox>();

var host = builder.Build();
host.Run();
