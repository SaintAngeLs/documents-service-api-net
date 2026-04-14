namespace Documents.Infrastructure.PostgreSQL.Entities;

public sealed class IntegrationCheckpoint
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public DateTime ProcessedAtUtc { get; set; }
}