namespace Documents.Application.Documents.Commands.CreateDocument;

public sealed class CreateDocumentItemDto
{
    public string ArticleName { get; set; } = string.Empty;
    public decimal TaxRate { get; set; }
    public decimal NetValue { get; set; }
}