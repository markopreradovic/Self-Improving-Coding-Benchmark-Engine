using Benchmark.Domain.Evaluation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Benchmark.Infrastructure.Persistence.Configurations;

public class EvaluationResultConfiguration : IEntityTypeConfiguration<EvaluationResult>
{
    public void Configure(EntityTypeBuilder<EvaluationResult> builder)
    {
        builder.ToTable("Evaluations");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.ModelName).HasMaxLength(100).IsRequired();
        builder.Property(e => e.GeneratedCode).HasColumnType("TEXT");
        builder.Property(e => e.OverallVerdict).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(e => e.Score).IsRequired();

        builder.OwnsMany(e => e.TestCaseResults, tcr =>
        {
            tcr.ToTable("EvaluationTestCaseResults");
            tcr.WithOwner().HasForeignKey("EvaluationId");
            tcr.Property<Guid>("Id").ValueGeneratedOnAdd();
            tcr.HasKey("Id");

            tcr.Property(t => t.Input).HasMaxLength(2000).IsRequired();
            tcr.Property(t => t.ExpectedOutput).HasMaxLength(1000).IsRequired();
            tcr.Property(t => t.ActualOutput).HasMaxLength(1000);
            tcr.Property(t => t.Verdict).HasConversion<string>().HasMaxLength(30).IsRequired();
            tcr.Property(t => t.ErrorMessage).HasMaxLength(2000);
        });

        builder.Navigation(e => e.TestCaseResults)
            .HasField("_testCaseResults")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(e => e.ProblemId);
        builder.HasIndex(e => e.ModelName);
        builder.HasIndex(e => e.EvaluatedAt);
    }
}
