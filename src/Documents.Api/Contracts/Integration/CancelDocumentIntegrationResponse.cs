namespace Documents.Api.Contracts.Integration;

/// <summary>
/// Odpowiedź zwracana po cofnięciu stanu integracji dokumentu.
/// </summary>
public sealed class CancelDocumentIntegrationResponse
{
    /// <summary>
    /// Identyfikator dokumentu.
    /// </summary>
    public Guid DocumentId { get; set; }

    /// <summary>
    /// Informacja, czy dokument jest aktualnie oznaczony jako zintegrowany.
    /// Po udanym cofnięciu integracji wartość powinna być równa <c>false</c>.
    /// </summary>
    public bool IsIntegrated { get; set; }

    /// <summary>
    /// Data i czas integracji dokumentu w UTC.
    /// Po cofnięciu integracji wartość może być pusta.
    /// </summary>
    public DateTime? IntegratedAtUtc { get; set; }
}