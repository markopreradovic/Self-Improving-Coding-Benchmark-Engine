using Benchmark.Domain.Problems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Benchmark.Infrastructure.Persistence.Configurations;

public class CodingProblemConfiguration : IEntityTypeConfiguration<CodingProblem>
{
    public void Configure(EntityTypeBuilder<CodingProblem> builder)
    {
        builder.ToTable("Problems");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(p => p.FunctionSignature)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(p => p.Category)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Difficulty)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.OwnsMany(p => p.TestCases, tc =>
        {
            tc.ToTable("TestCases");

            tc.WithOwner().HasForeignKey("ProblemId");

            tc.Property<Guid>("Id").ValueGeneratedOnAdd();
            tc.HasKey("Id");

            tc.Property(t => t.Input)
                .HasMaxLength(2000)
                .IsRequired();

            tc.Property(t => t.ExpectedOutput)
                .HasMaxLength(1000)
                .IsRequired();

            tc.Property(t => t.IsHidden)
                .IsRequired();

            tc.Property(t => t.Explanation)
                .HasMaxLength(1000);
        });

        builder.Navigation(p => p.TestCases)
            .HasField("_testCases")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(p => p.Category);
        builder.HasIndex(p => p.Difficulty);
        builder.HasIndex(p => p.CreatedAt);
    }
}
