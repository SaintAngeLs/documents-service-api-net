namespace Documents.Infrastructure.PostgreSQL.Documents.Entities;

public sealed class DocumentItemEntity
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public string ArticleName { get; set; } = default!;
    public decimal TaxRate { get; set; }
    public decimal NetValue { get; set; }

    public DocumentEntity Document { get; set; } = default!;
}