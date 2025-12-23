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
builder.Services.AddSingleton<ILlmClient>(sp => new OpenAiClient("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"));
builder.Services.AddSingleton<TrainingOrchestrator>();
builder.Services.AddSingleton<CodeEvaluator>();
builder.Services.AddSingleton<CodeSandbox>();

var host = builder.Build();
host.Run();
