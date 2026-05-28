using Benchmark.Domain.Training;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Benchmark.Infrastructure.Persistence.Configurations;

public class ModelVersionConfiguration : IEntityTypeConfiguration<ModelVersion>
{
    public void Configure(EntityTypeBuilder<ModelVersion> builder)
    {
        builder.ToTable("ModelVersions");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.ModelName).HasMaxLength(200).IsRequired();
        builder.Property(v => v.BaseModel).HasMaxLength(200).IsRequired();
        builder.Property(v => v.FilePath).HasMaxLength(500);

        builder.HasIndex(v => v.ModelName);
        builder.HasIndex(v => v.IsActive);
    }
}
