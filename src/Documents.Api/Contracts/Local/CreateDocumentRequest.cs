namespace Documents.Api.Contracts.Local;

public sealed class CreateDocumentRequest
{
    public string DescriptiveNo { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public List<CreateDocumentItemRequest> Items { get; set; } = [];
}