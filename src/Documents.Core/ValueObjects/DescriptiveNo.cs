using Documents.Core.Documents.Exceptions;

namespace Documents.Core.Documents.ValueObjects;

public readonly record struct DescriptiveNo
{
    public string Value { get; }

    public DescriptiveNo(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyDescriptiveNoException();
        }

        Value = value.Trim();
    }

    public override string ToString() => Value;

    public static implicit operator string(DescriptiveNo descriptiveNo)
        => descriptiveNo.Value;

    public static implicit operator DescriptiveNo(string value)
        => new(value);
}