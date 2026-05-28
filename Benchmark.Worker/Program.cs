using Benchmark.Application.Evaluation;
using Benchmark.Application.Features.Problems.Commands;
using Benchmark.Application.Generators;
using Benchmark.Domain.Evaluation;
using Benchmark.Domain.Sandbox;
using Benchmark.Infrastructure;
using Benchmark.ML;
using Benchmark.Sandbox.Execution;
using Benchmark.Worker;
using Benchmark.Worker.Jobs;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<WorkerOptions>(
    builder.Configuration.GetSection(WorkerOptions.Section));

builder.Services.AddScoped<ITypedProblemGenerator, ArrayProblemGenerator>();
builder.Services.AddScoped<ITypedProblemGenerator, StringProblemGenerator>();
builder.Services.AddScoped<ITypedProblemGenerator, TreeProblemGenerator>();
builder.Services.AddScoped<ITypedProblemGenerator, GraphProblemGenerator>();
builder.Services.AddScoped<ITypedProblemGenerator, DynamicProgrammingProblemGenerator>();
builder.Services.AddScoped<ITypedProblemGenerator, MathProblemGenerator>();
builder.Services.AddScoped<IProblemGenerator, ProblemGeneratorRouter>();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ICodeRunner, CodeRunner>();
builder.Services.AddScoped<IEvaluator, CodeEvaluator>();
builder.Services.AddMlServices(builder.Configuration);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(typeof(GenerateProblemCommand).Assembly));

builder.Services.AddScoped<ProblemGenerationJob>();
builder.Services.AddScoped<EvaluationJob>();
builder.Services.AddScoped<TrainingJob>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();
