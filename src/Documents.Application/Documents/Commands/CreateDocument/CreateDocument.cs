namespace Documents.Application.Documents.Commands.CreateDocument;

public sealed class CreateDocument
{
    public string DescriptiveNo { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public List<CreateDocumentItemDto> Items { get; set; } = [];
}