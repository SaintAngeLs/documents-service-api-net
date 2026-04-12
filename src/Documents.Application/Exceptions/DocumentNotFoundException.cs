using Documents.Core.Documents.ValueObjects;

namespace Documents.Application.Exceptions;

public sealed class DocumentNotFoundException : ApplicationException
{
    public override string Code => "document_not_found";

    public DocumentNotFoundException(DocumentId id)
        : base($"Document with id '{id}' was not found.")
    {
    }
}