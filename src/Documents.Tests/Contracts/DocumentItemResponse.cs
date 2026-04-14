namespace Documents.Tests.Contracts;

public sealed class DocumentItemResponse
{
    public Guid Id { get; set; }
    public string ArticleName { get; set; } = default!;
    public decimal TaxRate { get; set; }
    public decimal NetValue { get; set; }
}