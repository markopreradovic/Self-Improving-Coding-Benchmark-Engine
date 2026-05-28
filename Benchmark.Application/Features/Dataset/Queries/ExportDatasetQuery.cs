using Benchmark.Domain.Dataset;
using Benchmark.Domain.Problems;
using MediatR;

namespace Benchmark.Application.Features.Dataset.Queries;

public record ExportDatasetQuery(
    ExportFormat Format,
    ProblemCategory? Category = null,
    DifficultyLevel? Difficulty = null,
    SampleType? SampleType = null,
    double MinScore = 0.0) : IRequest<string>;
