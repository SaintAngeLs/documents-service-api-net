namespace Documents.Core.Common.ValueObjects;

public readonly record struct NullableUtcDateTime
{
    public DateTime? Value { get; }

    public bool HasValue => Value.HasValue;

    public NullableUtcDateTime(DateTime? value)
    {
        if (!value.HasValue)
        {
            Value = null;
            return;
        }

        Value = value.Value.Kind switch
        {
            DateTimeKind.Utc => value.Value,
            DateTimeKind.Local => value.Value.ToUniversalTime(),
            DateTimeKind.Unspecified => DateTime.SpecifyKind(value.Value, DateTimeKind.Utc),
            _ => DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
        };
    }

    public override string ToString()
        => Value?.ToString("O") ?? string.Empty;

    public static implicit operator DateTime?(NullableUtcDateTime utcDateTime)
        => utcDateTime.Value;

    public static implicit operator NullableUtcDateTime(DateTime? value)
        => new(value);
}