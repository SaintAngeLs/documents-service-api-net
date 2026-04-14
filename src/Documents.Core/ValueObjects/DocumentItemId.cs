namespace Documents.Core.Documents.ValueObjects;

public readonly record struct DocumentItemId(Guid Value)
{
    public static DocumentItemId Create()
        => new(Guid.NewGuid());

    public static implicit operator Guid(DocumentItemId id)
        => id.Value;

    public static implicit operator DocumentItemId(Guid value)
        => new(value);

    public override string ToString()
        => Value.ToString();
}