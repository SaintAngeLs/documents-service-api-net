using Documents.Infrastructure.PostgreSQL.Documents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Documents.Infrastructure.PostgreSQL.Documents.Configurations;

public sealed class DocumentItemEntityConfiguration : IEntityTypeConfiguration<DocumentItemEntity>
{
    public void Configure(EntityTypeBuilder<DocumentItemEntity> builder)
    {
        builder.ToTable("document_items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.DocumentId)
            .IsRequired();

        builder.Property(x => x.ArticleName)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.TaxRate)
            .HasColumnType("numeric(9,4)")
            .IsRequired();

        builder.Property(x => x.NetValue)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.HasIndex(x => x.DocumentId);
    }
}