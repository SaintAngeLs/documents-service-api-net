using Documents.Core.Common.ValueObjects;
using Documents.Core.Documents.Entities;
using Documents.Core.Documents.ValueObjects;
using Documents.Infrastructure.PostgreSQL.Documents.Entities;

namespace Documents.Infrastructure.PostgreSQL.Documents.Mappings;
internal static class DocumentEntityMapper
{
    public static Document ToDomain(DocumentEntity entity)
    {
        var document = new Document(
            new DocumentId(entity.Id),
            new DescriptiveNo(entity.DescriptiveNo),
            DocumentKind.From(entity.Kind),
            new UtcDateTime(entity.CreatedAtUtc),
            new Revision(entity.Revision));

        if (entity.IntegratedAtUtc.HasValue)
        {
            document.MarkAsIntegrated(new UtcDateTime(entity.IntegratedAtUtc.Value));
        }

        foreach (var item in entity.Items)
        {
            document.AddItem(new DocumentItem(
                new DocumentItemId(item.Id),
                new DocumentId(item.DocumentId),
                new ArticleName(item.ArticleName),
                new TaxRate(item.TaxRate),
                new NetValue(item.NetValue)));
        }

        return document;
    }

    public static DocumentEntity ToEntity(Document domain)
    {
        return new DocumentEntity
        {
            Id = domain.Id.Value,
            DescriptiveNo = domain.DescriptiveNo.Value,
            Kind = domain.Kind.Value,
            CreatedAtUtc = domain.CreatedAtUtc.Value,
            IntegratedAtUtc = domain.IntegratedAtUtc.Value,
            Revision = domain.Revision.Value,
            Items = domain.Items.Select(x => new DocumentItemEntity
            {
                Id = x.Id.Value,
                DocumentId = x.DocumentId.Value,
                ArticleName = x.ArticleName.Value,
                TaxRate = x.TaxRate.Value,
                NetValue = x.NetValue.Value
            }).ToList()
        };
    }
}