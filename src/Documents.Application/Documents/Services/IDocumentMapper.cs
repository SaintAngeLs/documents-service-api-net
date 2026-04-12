using Documents.Application.Documents.DTO;
using Documents.Core.Documents.Entities;

namespace Documents.Application.Documents.Services;

public interface IDocumentMapper
{
    DocumentDto Map(Document document);
    DocumentDetailsDto MapDetails(Document document);
    DocumentIntegrationStatusDto MapIntegrationStatus(Document document);
}