namespace Documents.Api.Contracts.Integration;

public sealed class GetDocumentsAggregateResponse
{
    public DateTime GeneratedAtUtc { get; set; }
    public List<DocumentAggregateRowResponse> Rows { get; set; } = [];
}