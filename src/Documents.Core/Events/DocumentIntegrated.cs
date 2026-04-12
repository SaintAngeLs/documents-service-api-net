using Documents.Core.Common.ValueObjects;
using Documents.Core.Documents.ValueObjects;
using Documents.Core.Events;

namespace Documents.Core.Documents.Events;

public sealed class DocumentIntegrated : IDomainEvent
{
    public DocumentId DocumentId { get; }
    public UtcDateTime IntegratedAtUtc { get; }

    public DocumentIntegrated(DocumentId documentId, UtcDateTime integratedAtUtc)
    {
        DocumentId = documentId;
        IntegratedAtUtc = integratedAtUtc;
    }
}