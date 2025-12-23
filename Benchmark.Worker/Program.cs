using Benchmark.Engine.Dataset;
using Benchmark.Engine.Evaluation;
using Benchmark.Engine.Runner;
using Benchmark.Engine.Sandbox;
using Benchmark.ML.LLM;
using Benchmark.ML.Training;
using Benchmark.Worker;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// ENGINE
builder.Services.AddSingleton<CodeSandbox>();
builder.Services.AddSingleton<CodeEvaluator>();
builder.Services.AddSingleton<DatasetBuilder>();
builder.Services.AddSingleton<BenchmarkRunner>();

// ML
builder.Services.AddSingleton<ILlmClient, OpenAiClient>();
builder.Services.AddSingleton<TrainingOrchestrator>();

// Worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
