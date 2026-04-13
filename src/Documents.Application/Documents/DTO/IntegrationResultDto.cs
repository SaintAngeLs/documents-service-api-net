namespace Documents.Application.Documents.DTO;

public sealed class IntegrationResultDto
{
    public int IntegratedDocumentsCount { get; set; }
    public DateTime IntegratedAtUtc { get; set; }
    public List<Guid> DocumentIds { get; set; } = [];
}