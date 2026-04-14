using Documents.Core.Abstractions;
using Documents.Core.Common.ValueObjects;
using Documents.Core.Documents.Events;
using Documents.Core.Documents.Exceptions;
using Documents.Core.Documents.ValueObjects;

namespace Documents.Core.Documents.Entities;

public sealed class Document : AggregateRoot
{
    private readonly List<DocumentItem> _items = new();

    public DocumentId Id { get; }
    public DescriptiveNo DescriptiveNo { get; private set; }
    public DocumentKind Kind { get; private set; }
    public UtcDateTime CreatedAtUtc { get; }
    public NullableUtcDateTime IntegratedAtUtc { get; private set; }
    public Revision Revision { get; private set; }

    public IReadOnlyCollection<DocumentItem> Items => _items.AsReadOnly();

    public Document(
        DocumentId id,
        DescriptiveNo descriptiveNo,
        DocumentKind kind,
        UtcDateTime createdAtUtc,
        Revision revision)
    {
        Id = id.Equals(default) ? DocumentId.Create() : id;
        DescriptiveNo = descriptiveNo;
        Kind = kind;
        CreatedAtUtc = createdAtUtc;
        Revision = revision;
        IntegratedAtUtc = new NullableUtcDateTime(null);

        AddEvent(new DocumentCreated(Id));
    }

    public static Document Create(
        DescriptiveNo descriptiveNo,
        DocumentKind kind,
        UtcDateTime createdAtUtc)
        => new(
            DocumentId.Create(),
            descriptiveNo,
            kind,
            createdAtUtc,
            new Revision(0));

    public static Document Restore(
        DocumentId id,
        DescriptiveNo descriptiveNo,
        DocumentKind kind,
        UtcDateTime createdAtUtc,
        NullableUtcDateTime integratedAtUtc,
        Revision revision,
        IEnumerable<DocumentItem> items)
    {
        var document = new Document(id, descriptiveNo, kind, createdAtUtc, revision);

        document.IntegratedAtUtc = integratedAtUtc;

        foreach (var item in items)
        {
            document._items.Add(item);
        }

        document.ClearEvents();
        return document;
    }

    public void ChangeDescriptiveNo(DescriptiveNo descriptiveNo)
    {
        EnsureEditable();
        DescriptiveNo = descriptiveNo;
    }

    public void ChangeKind(DocumentKind kind)
    {
        EnsureEditable();

        foreach (var item in _items)
        {
            EnsureNetValueMatchesKind(kind, item.NetValue);
        }

        Kind = kind;
    }

    public void AddItem(DocumentItem item)
    {
        EnsureEditable();
        EnsureNetValueMatchesKind(Kind, item.NetValue);

        _items.Add(item);
        AddEvent(new DocumentItemAdded(Id, item.Id));
    }

    public void RemoveItem(DocumentItemId itemId)
    {
        EnsureEditable();

        var item = _items.SingleOrDefault(x => x.Id.Equals(itemId));
        if (item is null)
        {
            return;
        }

        _items.Remove(item);
        AddEvent(new DocumentItemRemoved(Id, item.Id));
    }

    public void UpdateItem(
        DocumentItemId itemId,
        ArticleName articleName,
        TaxRate taxRate,
        NetValue netValue)
    {
        EnsureEditable();
        EnsureNetValueMatchesKind(Kind, netValue);

        var item = _items.SingleOrDefault(x => x.Id.Equals(itemId));
        if (item is null)
        {
            return;
        }

        item.Update(articleName, taxRate, netValue);
    }

    public void ValidateBeforeSave()
    {
        if (_items.Count == 0)
        {
            throw new CannotSaveDocumentWithoutItemsException();
        }
    }

    public void MarkAsIntegrated(UtcDateTime integratedAtUtc)
    {
        EnsureEditable();

        IntegratedAtUtc = new NullableUtcDateTime(integratedAtUtc.Value);
        AddEvent(new DocumentIntegrated(Id, integratedAtUtc));
    }

    public void CancelIntegration()
    {
        IntegratedAtUtc = new NullableUtcDateTime(null);
    }

    public void IncreaseRevision()
    {
        Revision = Revision.Next();
    }

    private void EnsureEditable()
    {
        if (IntegratedAtUtc.HasValue)
        {
            throw new CannotModifyIntegratedDocumentException(Id);
        }
    }

    private static void EnsureNetValueMatchesKind(DocumentKind kind, NetValue netValue)
    {
        var isValid = kind switch
        {
            _ when kind.IsReturn => netValue.IsNegative(),
            _ when kind.IsInvoice || kind.IsReceipt => netValue.IsPositive(),
            _ => false
        };

        if (!isValid)
        {
            throw new InvalidDocumentItemNetValueException(kind, netValue);
        }
    }
}