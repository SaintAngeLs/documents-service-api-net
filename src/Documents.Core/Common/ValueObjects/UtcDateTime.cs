namespace Documents.Core.Common.ValueObjects;

public readonly record struct UtcDateTime
{
    public DateTime Value { get; }

    public UtcDateTime(DateTime value)
    {
        Value = value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    }

    public static UtcDateTime Now()
        => new(DateTime.UtcNow);

    public override string ToString()
        => Value.ToString("O");

    public static implicit operator DateTime(UtcDateTime utcDateTime)
        => utcDateTime.Value;

    public static implicit operator UtcDateTime(DateTime value)
        => new(value);
}