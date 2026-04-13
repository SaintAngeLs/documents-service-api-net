namespace Documents.Api.Contracts.Integration;

public sealed class IntegrateDocumentResponse
{
    public Guid DocumentId { get; set; }
    public bool IsIntegrated { get; set; }
    public DateTime? IntegratedAtUtc { get; set; }
}