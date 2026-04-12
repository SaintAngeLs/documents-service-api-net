using Documents.Core.Documents.Exceptions;

namespace Documents.Core.Documents.ValueObjects;

public readonly record struct ArticleName
{
    public string Value { get; }

    public ArticleName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyArticleNameException();
        }

        Value = value.Trim();
    }

    public override string ToString() => Value;

    public static implicit operator string(ArticleName articleName)
        => articleName.Value;

    public static implicit operator ArticleName(string value)
        => new(value);
}