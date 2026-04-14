namespace Documents.Api.Contracts.Integration;

/// <summary>
/// Pojedynczy wiersz agregacji dokumentów dla integratora.
/// </summary>
public sealed class DocumentAggregateRowResponse
{
    /// <summary>
    /// Typ dokumentu, np. Invoice, Receipt albo Return.
    /// </summary>
    public string Kind { get; set; } = string.Empty;

    /// <summary>
    /// Stawka podatku, dla której została policzona agregacja.
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// Łączna suma wartości netto dla wskazanego typu dokumentu i stawki podatku.
    /// </summary>
    public decimal TotalNetValue { get; set; }
}