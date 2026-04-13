using Documents.Core.Common.ValueObjects;
using Documents.Core.Documents.Entities;
using Documents.Core.Documents.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Documents.Infrastructure.Persistence.Configurations;

public sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("documents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new DocumentId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.DescriptiveNo)
            .HasConversion(
                value => value.Value,
                value => new DescriptiveNo(value))
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Kind)
            .HasConversion(
                value => value.Value,
                value => DocumentKind.From(value))
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasConversion(
                value => value.Value,
                value => new UtcDateTime(value))
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(x => x.IntegratedAtUtc)
            .HasConversion(
                value => value.Value,
                value => new NullableUtcDateTime(value))
            .HasColumnType("timestamp with time zone");

        builder.Property(x => x.Revision)
            .HasConversion(
                value => value.Value,
                value => new Revision(value))
            .IsRequired();

        builder.Ignore("_events");
        builder.Ignore(x => x.Events);

        builder.Metadata.FindNavigation(nameof(Document.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(x => x.Items)
            .WithOne(x => x.Document)
            .HasForeignKey(x => x.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.DescriptiveNo);
        builder.HasIndex(x => x.Kind);
        builder.HasIndex(x => x.IntegratedAtUtc);
    }
}