using Documents.Core.Abstractions;

namespace Documents.Core.Documents.Exceptions;

public sealed class CannotSaveDocumentWithoutItemsException : DomainException
{
    public override string Code => "cannot_save_document_without_items";

    public CannotSaveDocumentWithoutItemsException()
        : base("Document cannot be saved without at least one item.")
    {
    }
}