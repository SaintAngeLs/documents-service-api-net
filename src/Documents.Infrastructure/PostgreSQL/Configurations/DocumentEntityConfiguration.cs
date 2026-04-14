using Documents.Infrastructure.PostgreSQL.Documents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Documents.Infrastructure.PostgreSQL.Documents.Configurations;

public sealed class DocumentEntityConfiguration : IEntityTypeConfiguration<DocumentEntity>
{
    public void Configure(EntityTypeBuilder<DocumentEntity> builder)
    {
        builder.ToTable("documents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.DescriptiveNo)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Kind)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(x => x.IntegratedAtUtc)
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.Revision)
            .IsRequired();

        builder.HasMany(x => x.Items)
            .WithOne(x => x.Document)
            .HasForeignKey(x => x.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.DescriptiveNo);
        builder.HasIndex(x => x.Kind);
        builder.HasIndex(x => x.IntegratedAtUtc);
    }
}