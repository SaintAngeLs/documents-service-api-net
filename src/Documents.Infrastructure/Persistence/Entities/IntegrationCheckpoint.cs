namespace Documents.Infrastructure.Persistence.Entities;

public sealed class IntegrationCheckpoint
{
    public Guid Id { get; set; }
    public DateTime? LastProcessedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}