namespace Documents.Core.Documents.ValueObjects;

public readonly record struct DocumentId(Guid Value)
{
    public static DocumentId Create()
        => new(Guid.NewGuid());

    public static implicit operator Guid(DocumentId id)
        => id.Value;

    public static implicit operator DocumentId(Guid value)
        => new(value);

    public override string ToString()
        => Value.ToString();
}