namespace Documents.Infrastructure.PostgreSQL.Documents.Entities;

public sealed class DocumentEntity
{
    public Guid Id { get; set; }
    public string DescriptiveNo { get; set; } = default!;
    public string Kind { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? IntegratedAtUtc { get; set; }
    public int Revision { get; set; }

    public List<DocumentItemEntity> Items { get; set; } = new();
}