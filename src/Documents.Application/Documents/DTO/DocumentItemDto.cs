namespace Documents.Application.Documents.DTO;

public sealed class DocumentItemDto
{
    public Guid Id { get; set; }
    public string ArticleName { get; set; } = string.Empty;
    public decimal TaxRate { get; set; }
    public decimal NetValue { get; set; }
}