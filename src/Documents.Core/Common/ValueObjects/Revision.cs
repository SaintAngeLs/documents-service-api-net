namespace Documents.Core.Common.ValueObjects;

public readonly record struct Revision
{
    public int Value { get; }

    public Revision(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value, "Revision cannot be negative.");
        }

        Value = value;
    }

    public Revision Next()
        => new(Value + 1);

    public override string ToString()
        => Value.ToString();

    public static implicit operator int(Revision revision)
        => revision.Value;

    public static implicit operator Revision(int value)
        => new(value);
}