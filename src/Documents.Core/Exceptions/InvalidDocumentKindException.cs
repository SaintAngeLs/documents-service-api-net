using Documents.Core.Abstractions;

namespace Documents.Core.Documents.Exceptions;

public sealed class InvalidDocumentKindException : DomainException
{
    public override string Code => "invalid_document_kind";

    public InvalidDocumentKindException(string value)
        : base($"Document kind '{value}' is invalid.")
    {
    }
}