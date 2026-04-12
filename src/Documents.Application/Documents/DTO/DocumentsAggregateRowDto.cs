namespace Documents.Application.Documents.DTO;

public sealed class DocumentsAggregateRowDto
{
    public string Kind { get; set; } = string.Empty;
    public decimal TaxRate { get; set; }
    public decimal TotalNetValue { get; set; }
}