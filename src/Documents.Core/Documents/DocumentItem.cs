using Documents.Core.Documents.ValueObjects;

namespace Documents.Core.Documents.Entities;

public sealed class DocumentItem
{
    public DocumentItemId Id { get; }
    public DocumentId DocumentId { get; private set; }
    public Document Document { get; private set; } = default!;

    public ArticleName ArticleName { get; private set; }
    public TaxRate TaxRate { get; private set; }
    public NetValue NetValue { get; private set; }

    public DocumentItem(
        DocumentItemId id,
        DocumentId documentId,
        ArticleName articleName,
        TaxRate taxRate,
        NetValue netValue)
    {
        Id = id.Equals(default) ? DocumentItemId.Create() : id;
        DocumentId = documentId;
        ArticleName = articleName;
        TaxRate = taxRate;
        NetValue = netValue;
    }

    public void Update(
        ArticleName articleName,
        TaxRate taxRate,
        NetValue netValue)
    {
        ArticleName = articleName;
        TaxRate = taxRate;
        NetValue = netValue;
    }
}