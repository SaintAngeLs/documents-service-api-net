using Documents.Core.Abstractions;

namespace Documents.Core.Documents.Exceptions;

public sealed class EmptyArticleNameException : DomainException
{
    public override string Code => "empty_article_name";

    public EmptyArticleNameException()
        : base("Document item article name cannot be empty.")
    {
    }
}