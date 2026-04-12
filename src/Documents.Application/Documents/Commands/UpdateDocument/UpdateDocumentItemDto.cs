namespace Documents.Application.Documents.Commands.UpdateDocument;

public sealed class UpdateDocumentItemDto
{
    public Guid Id { get; set; }
    public string ArticleName { get; set; } = string.Empty;
    public decimal TaxRate { get; set; }
    public decimal NetValue { get; set; }
}