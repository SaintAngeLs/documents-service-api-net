namespace Documents.Tests.Contracts;

public sealed class DocumentDetailsResponse
{
    public Guid Id { get; set; }
    public string DescriptiveNo { get; set; } = default!;
    public string Kind { get; set; } = default!;
    public int Revision { get; set; }
    public List<DocumentItemResponse> Items { get; set; } = [];
}