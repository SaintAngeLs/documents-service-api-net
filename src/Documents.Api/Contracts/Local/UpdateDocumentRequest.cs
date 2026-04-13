namespace Documents.Api.Contracts.Local;

public sealed class UpdateDocumentRequest
{
    public string DescriptiveNo { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public int Revision { get; set; }
    public List<UpdateDocumentItemRequest> Items { get; set; } = [];
}