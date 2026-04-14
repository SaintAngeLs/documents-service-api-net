using Documents.Core.Documents.Exceptions;

namespace Documents.Core.Documents.ValueObjects;

public readonly record struct DocumentKind
{
    public string Value { get; }

    private DocumentKind(string value)
    {
        Value = value;
    }

    public static DocumentKind Invoice => new("Invoice");
    public static DocumentKind Receipt => new("Receipt");
    public static DocumentKind Return => new("Return");

    public bool IsInvoice => Value.Equals("Invoice", StringComparison.OrdinalIgnoreCase);
    public bool IsReceipt => Value.Equals("Receipt", StringComparison.OrdinalIgnoreCase);
    public bool IsReturn => Value.Equals("Return", StringComparison.OrdinalIgnoreCase);

    public static DocumentKind From(string value)
    {
        var normalized = value?.Trim().ToUpperInvariant();

        return normalized switch
        {
            "INVOICE" => Invoice,
            "RECEIPT" => Receipt,
            "RETURN" => Return,
            _ => throw new InvalidDocumentKindException(value ?? string.Empty)
        };
    }

    public override string ToString() => Value;

    public static implicit operator string(DocumentKind kind) => kind.Value;
}