using Documents.Application.Documents.DTO;
using Documents.Application.Documents.Services;
using Documents.Application.Exceptions;
using Documents.Core.Documents.Repositories;
using Documents.Core.Documents.ValueObjects;

namespace Documents.Application.Documents.Queries.GetDocumentIntegrationStatus;

public sealed class GetDocumentIntegrationStatusHandler
{
    private readonly IDocumentsRepository _documentsRepository;
    private readonly IDocumentMapper _mapper;

    public GetDocumentIntegrationStatusHandler(
        IDocumentsRepository documentsRepository,
        IDocumentMapper mapper)
    {
        _documentsRepository = documentsRepository;
        _mapper = mapper;
    }

    public async Task<DocumentIntegrationStatusDto> HandleAsync(
        GetDocumentIntegrationStatus query,
        CancellationToken cancellationToken = default)
    {
        var documentId = new DocumentId(query.Id);
        var document = await _documentsRepository.GetAsync(documentId);

        if (document is null)
        {
            throw new DocumentNotFoundException(documentId);
        }

        return _mapper.MapIntegrationStatus(document);
    }
}