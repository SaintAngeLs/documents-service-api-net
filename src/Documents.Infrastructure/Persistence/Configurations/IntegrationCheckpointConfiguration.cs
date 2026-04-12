using Documents.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Documents.Infrastructure.Persistence.Configurations;

public sealed class IntegrationCheckpointConfiguration : IEntityTypeConfiguration<IntegrationCheckpoint>
{
    public void Configure(EntityTypeBuilder<IntegrationCheckpoint> builder)
    {
        builder.ToTable("integration_checkpoints");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(x => x.LastProcessedAtUtc)
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);
    }
}