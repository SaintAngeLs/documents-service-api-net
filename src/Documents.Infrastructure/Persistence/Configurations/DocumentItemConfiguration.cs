using Documents.Core.Documents.Entities;
using Documents.Core.Documents.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Documents.Infrastructure.Persistence.Configurations;

public sealed class DocumentItemConfiguration : IEntityTypeConfiguration<DocumentItem>
{
    public void Configure(EntityTypeBuilder<DocumentItem> builder)
    {
        builder.ToTable("document_items");

        builder.HasKey(x => x.Id);

        builder.Property<Guid>("DocumentId")
            .IsRequired();

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new DocumentItemId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.ArticleName)
            .HasConversion(
                value => value.Value,
                value => new ArticleName(value))
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.TaxRate)
            .HasConversion(
                value => value.Value,
                value => new TaxRate(value))
            .HasColumnType("numeric(9,4)")
            .IsRequired();

        builder.Property(x => x.NetValue)
            .HasConversion(
                value => value.Value,
                value => new NetValue(value))
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.HasIndex("DocumentId");
    }
}