namespace Documents.Api.Contracts.Integration;

/// <summary>
/// Odpowiedź zawierająca zagregowane dane dokumentów dla integratora.
/// </summary>
public sealed class GetDocumentsAggregateResponse
{
    /// <summary>
    /// Czas wygenerowania agregatu w UTC.
    /// </summary>
    public DateTime GeneratedAtUtc { get; set; }

    /// <summary>
    /// Wiersze agregacji grupowane po typie dokumentu i stawce podatku.
    /// </summary>
    public List<DocumentAggregateRowResponse> Rows { get; set; } = [];
}