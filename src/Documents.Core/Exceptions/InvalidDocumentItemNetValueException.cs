using Documents.Core.Abstractions;
using Documents.Core.Documents.ValueObjects;

namespace Documents.Core.Documents.Exceptions;

public sealed class InvalidDocumentItemNetValueException : DomainException
{
    public override string Code => "invalid_document_item_net_value";

    public InvalidDocumentItemNetValueException(DocumentKind kind, decimal netValue)
        : base($"Net value '{netValue}' is invalid for document kind '{kind}'.")
    {
    }
}