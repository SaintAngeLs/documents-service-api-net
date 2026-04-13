namespace Documents.Api.Contracts.Integration;

public sealed class IntegrateAllDocumentsResponse
{
    public int IntegratedDocumentsCount { get; set; }
    public DateTime IntegratedAtUtc { get; set; }
    public List<Guid> DocumentIds { get; set; } = [];
}