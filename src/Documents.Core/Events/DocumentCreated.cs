using Documents.Core.Documents.ValueObjects;
using Documents.Core.Events;

namespace Documents.Core.Documents.Events;

public sealed class DocumentCreated : IDomainEvent
{
    public DocumentId DocumentId { get; }

    public DocumentCreated(DocumentId documentId)
    {
        DocumentId = documentId;
    }
}