using Documents.Core.Documents.ValueObjects;

namespace Documents.Core.Documents.Entities;

public sealed class DocumentItem
{
    public DocumentItemId Id { get; }
    public ArticleName ArticleName { get; private set; }
    public TaxRate TaxRate { get; private set; }
    public NetValue NetValue { get; private set; }

    public DocumentItem(
        DocumentItemId id,
        ArticleName articleName,
        TaxRate taxRate,
        NetValue netValue)
    {
        Id = id.Equals(default) ? DocumentItemId.Create() : id;
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