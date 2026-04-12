using Documents.Core.Documents.ValueObjects;
using Documents.Core.Events;

namespace Documents.Core.Documents.Events;

public sealed class DocumentItemRemoved : IDomainEvent
{
    public DocumentId DocumentId { get; }
    public DocumentItemId ItemId { get; }

    public DocumentItemRemoved(DocumentId documentId, DocumentItemId itemId)
    {
        DocumentId = documentId;
        ItemId = itemId;
    }
}