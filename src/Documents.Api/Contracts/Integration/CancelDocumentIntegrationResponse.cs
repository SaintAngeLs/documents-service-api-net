namespace Documents.Api.Contracts.Integration;

public sealed class CancelDocumentIntegrationResponse
{
    public Guid DocumentId { get; set; }
    public bool IsIntegrated { get; set; }
    public DateTime? IntegratedAtUtc { get; set; }
}