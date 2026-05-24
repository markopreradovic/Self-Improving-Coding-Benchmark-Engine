using System.Text.Json.Serialization;
using Benchmark.Api.Features.Problems;
using Benchmark.Api.Features.Sandbox;
using Benchmark.Api.Features.Solver;
using Benchmark.Application.Features.Problems.Commands;
using Benchmark.Application.Generators;
using Benchmark.Domain.Sandbox;
using Benchmark.Infrastructure;
using Benchmark.ML;
using Benchmark.Sandbox.Execution;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITypedProblemGenerator, ArrayProblemGenerator>();
builder.Services.AddScoped<ITypedProblemGenerator, StringProblemGenerator>();
builder.Services.AddScoped<ITypedProblemGenerator, TreeProblemGenerator>();
builder.Services.AddScoped<ITypedProblemGenerator, GraphProblemGenerator>();
builder.Services.AddScoped<ITypedProblemGenerator, DynamicProgrammingProblemGenerator>();
builder.Services.AddScoped<ITypedProblemGenerator, MathProblemGenerator>();
builder.Services.AddScoped<IProblemGenerator, ProblemGeneratorRouter>();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ICodeRunner, CodeRunner>();
builder.Services.AddMlServices(builder.Configuration);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(GenerateProblemCommand).Assembly);
});

var app = builder.Build();

await app.Services.ApplyMigrationsAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapProblemEndpoints();
app.MapSandboxEndpoints();
app.MapSolverEndpoints();

app.Run();
