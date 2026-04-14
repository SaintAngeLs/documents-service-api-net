namespace Documents.Api.Contracts.Integration;

public sealed class DocumentAggregateRowResponse
{
    public string Kind { get; set; } = string.Empty;
    public decimal TaxRate { get; set; }
    public decimal TotalNetValue { get; set; }
}