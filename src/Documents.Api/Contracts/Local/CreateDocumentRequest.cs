namespace Documents.Api.Contracts.Local;

/// <summary>
/// Żądanie utworzenia nowego dokumentu.
/// </summary>
public sealed class CreateDocumentRequest
{
    /// <summary>
    /// Numer opisowy dokumentu, np. FV/001/04/2026.
    /// </summary>
    public string DescriptiveNo { get; set; } = default!;

    /// <summary>
    /// Typ dokumentu: Invoice, Receipt lub Return.
    /// </summary>
    public string Kind { get; set; } = default!;

    /// <summary>
    /// Lista pozycji dokumentu. Dokument musi zawierać co najmniej jedną pozycję.
    /// </summary>
    public List<CreateDocumentItemRequest> Items { get; set; } = [];
}