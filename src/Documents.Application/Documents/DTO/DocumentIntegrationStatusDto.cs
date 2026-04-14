namespace Documents.Application.Documents.DTO;

public sealed class DocumentIntegrationStatusDto
{
    public Guid DocumentId { get; set; }
    public bool IsIntegrated { get; set; }
    public DateTime? IntegratedAtUtc { get; set; }
}