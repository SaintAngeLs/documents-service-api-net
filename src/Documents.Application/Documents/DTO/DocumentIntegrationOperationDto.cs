namespace Documents.Application.Documents.DTO;

public sealed class DocumentIntegrationOperationDto
{
    public Guid DocumentId { get; set; }
    public bool IsIntegrated { get; set; }
    public DateTime? IntegratedAtUtc { get; set; }
}