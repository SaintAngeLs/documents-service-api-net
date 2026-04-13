namespace Documents.Api.Contracts.Local;

public sealed class UpdateDocumentItemRequest
{
    public Guid Id { get; set; }
    public string ArticleName { get; set; } = string.Empty;
    public decimal TaxRate { get; set; }
    public decimal NetValue { get; set; }
}