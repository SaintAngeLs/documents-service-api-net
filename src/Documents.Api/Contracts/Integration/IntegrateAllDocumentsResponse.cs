namespace Documents.Api.Contracts.Integration;

/// <summary>
/// Odpowiedź zwracana po oznaczeniu wszystkich oczekujących dokumentów jako zintegrowane.
/// </summary>
public sealed class IntegrateAllDocumentsResponse
{
    /// <summary>
    /// Liczba dokumentów objętych operacją integracji.
    /// </summary>
    public int IntegratedDocumentsCount { get; set; }

    /// <summary>
    /// Data i czas wykonania operacji integracji w UTC.
    /// </summary>
    public DateTime IntegratedAtUtc { get; set; }

    /// <summary>
    /// Identyfikatory dokumentów objętych operacją integracji.
    /// </summary>
    public List<Guid> DocumentIds { get; set; } = [];
}