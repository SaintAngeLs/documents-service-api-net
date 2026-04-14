namespace Documents.Api.Contracts.Local;

/// <summary>
/// Pozycja tworzonego dokumentu.
/// </summary>
public sealed class CreateDocumentItemRequest
{
    /// <summary>
    /// Nazwa artykułu.
    /// </summary>
    public string ArticleName { get; set; } = default!;

    /// <summary>
    /// Stawka podatku.
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// Wartość netto pozycji. Dla Return powinna być ujemna, dla Invoice i Receipt dodatnia.
    /// </summary>
    public decimal NetValue { get; set; }
}