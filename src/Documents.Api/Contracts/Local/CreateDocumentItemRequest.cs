namespace Documents.Api.Contracts.Local;

public sealed class CreateDocumentItemRequest
{
    public string ArticleName { get; set; } = string.Empty;
    public decimal TaxRate { get; set; }
    public decimal NetValue { get; set; }
}