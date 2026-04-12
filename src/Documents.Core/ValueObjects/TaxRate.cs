using Documents.Core.Documents.Exceptions;

namespace Documents.Core.Documents.ValueObjects;

public readonly record struct TaxRate
{
    public decimal Value { get; }

    public TaxRate(decimal value)
    {
        if (value < 0)
        {
            throw new InvalidTaxRateException(value);
        }

        Value = value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator decimal(TaxRate taxRate)
        => taxRate.Value;

    public static implicit operator TaxRate(decimal value)
        => new(value);
}