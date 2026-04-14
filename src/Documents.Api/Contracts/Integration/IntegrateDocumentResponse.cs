namespace Documents.Api.Contracts.Integration;

/// <summary>
/// Odpowiedź zwracana po oznaczeniu pojedynczego dokumentu jako zintegrowany.
/// </summary>
public sealed class IntegrateDocumentResponse
{
    /// <summary>
    /// Identyfikator dokumentu.
    /// </summary>
    public Guid DocumentId { get; set; }

    /// <summary>
    /// Informacja, czy dokument jest oznaczony jako zintegrowany.
    /// </summary>
    public bool IsIntegrated { get; set; }

    /// <summary>
    /// Data i czas integracji dokumentu w UTC.
    /// </summary>
    public DateTime? IntegratedAtUtc { get; set; }
}