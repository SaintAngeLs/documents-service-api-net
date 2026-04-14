using Documents.Core.Documents.ValueObjects;

namespace Documents.Application.Exceptions;

public sealed class DocumentConcurrencyException : ApplicationException
{
    public override string Code => "document_concurrency_conflict";

    public DocumentConcurrencyException(DocumentId id)
        : base($"Document with id '{id}' has been modified by another process.")
    {
    }
}