using Benchmark.Domain.Dataset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Benchmark.Infrastructure.Persistence.Configurations;

public class FineTuneSampleConfiguration : IEntityTypeConfiguration<FineTuneSample>
{
    public void Configure(EntityTypeBuilder<FineTuneSample> builder)
    {
        builder.ToTable("FineTuneSamples");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.ProblemTitle).HasMaxLength(300).IsRequired();
        builder.Property(s => s.ProblemDescription).HasColumnType("TEXT").IsRequired();
        builder.Property(s => s.FunctionSignature).HasMaxLength(500).IsRequired();
        builder.Property(s => s.Code).HasColumnType("TEXT").IsRequired();
        builder.Property(s => s.ModelName).HasMaxLength(100).IsRequired();

        builder.Property(s => s.Category)
            .HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(s => s.Difficulty)
            .HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(s => s.SampleType)
            .HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(s => s.Verdict)
            .HasConversion<string>().HasMaxLength(30).IsRequired();

        builder.HasIndex(s => s.Category);
        builder.HasIndex(s => s.Difficulty);
        builder.HasIndex(s => s.SampleType);
        builder.HasIndex(s => s.EvaluationId);
    }
}
