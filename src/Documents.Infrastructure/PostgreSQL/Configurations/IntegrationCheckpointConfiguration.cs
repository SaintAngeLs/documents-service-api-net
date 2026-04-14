using Documents.Infrastructure.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Documents.Infrastructure.PostgreSQL.Configurations;

public sealed class IntegrationCheckpointConfiguration : IEntityTypeConfiguration<IntegrationCheckpoint>
{
    public void Configure(EntityTypeBuilder<IntegrationCheckpoint> builder)
    {
        builder.ToTable("integration_checkpoints");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.DocumentId)
            .IsRequired();

        builder.Property(x => x.ProcessedAtUtc)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasIndex(x => x.DocumentId)
            .IsUnique();
    }
}