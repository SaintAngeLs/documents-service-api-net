namespace Documents.Application.Documents.Commands.UpdateDocument;

public sealed class UpdateDocument
{
    public Guid Id { get; set; }
    public string DescriptiveNo { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public int Revision { get; set; }
    public List<UpdateDocumentItemDto> Items { get; set; } = [];
}