namespace Documents.Tests.Contracts;

public sealed class DocumentListItemResponse
{
    public Guid Id { get; set; }
    public string DescriptiveNo { get; set; } = default!;
    public string Kind { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}