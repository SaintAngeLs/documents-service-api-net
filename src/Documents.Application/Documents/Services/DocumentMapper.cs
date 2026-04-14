using Documents.Application.Documents.DTO;
using Documents.Core.Documents.Entities;

namespace Documents.Application.Documents.Services;

public sealed class DocumentMapper : IDocumentMapper
{
    public DocumentDto Map(Document document)
        => new()
        {
            Id = document.Id.Value,
            DescriptiveNo = document.DescriptiveNo.Value,
            Kind = document.Kind.Value,
            CreatedAtUtc = document.CreatedAtUtc.Value,
            IntegratedAtUtc = document.IntegratedAtUtc.Value,
            Revision = document.Revision.Value
        };

    public DocumentDetailsDto MapDetails(Document document)
        => new()
        {
            Id = document.Id.Value,
            DescriptiveNo = document.DescriptiveNo.Value,
            Kind = document.Kind.Value,
            CreatedAtUtc = document.CreatedAtUtc.Value,
            IntegratedAtUtc = document.IntegratedAtUtc.Value,
            Revision = document.Revision.Value,
            Items = document.Items.Select(x => new DocumentItemDto
            {
                Id = x.Id.Value,
                ArticleName = x.ArticleName.Value,
                TaxRate = x.TaxRate.Value,
                NetValue = x.NetValue.Value
            }).ToList()
        };

    public DocumentIntegrationStatusDto MapIntegrationStatus(Document document)
        => new()
        {
            DocumentId = document.Id.Value,
            IsIntegrated = document.IntegratedAtUtc.HasValue,
            IntegratedAtUtc = document.IntegratedAtUtc.Value
        };
}