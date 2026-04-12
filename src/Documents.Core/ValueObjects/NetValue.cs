namespace Documents.Core.Documents.ValueObjects;

public readonly record struct NetValue
{
    public decimal Value { get; }

    public NetValue(decimal value)
    {
        Value = value;
    }

    public bool IsPositive() => Value > 0;
    public bool IsNegative() => Value < 0;
    public bool IsZero() => Value == 0;

    public override string ToString()
        => Value.ToString();

    public static implicit operator decimal(NetValue netValue)
        => netValue.Value;

    public static implicit operator NetValue(decimal value)
        => new(value);
}