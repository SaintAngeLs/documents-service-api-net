using Documents.Core.Abstractions;
using Documents.Core.Documents.ValueObjects;

namespace Documents.Core.Documents.Exceptions;

public sealed class CannotModifyIntegratedDocumentException : DomainException
{
    public override string Code => "cannot_modify_integrated_document";

    public CannotModifyIntegratedDocumentException(DocumentId documentId)
        : base($"Document with id '{documentId}' cannot be modified because it was already integrated.")
    {
    }
}