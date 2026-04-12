using Documents.Application.Documents.DTO;
using Documents.Application.Documents.Services;
using Documents.Core.Documents.Repositories;

namespace Documents.Application.Documents.Queries.BrowseDocuments;

public sealed class BrowseDocumentsHandler
{
    private readonly IDocumentsRepository _documentsRepository;
    private readonly IDocumentMapper _mapper;

    public BrowseDocumentsHandler(
        IDocumentsRepository documentsRepository,
        IDocumentMapper mapper)
    {
        _documentsRepository = documentsRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<DocumentDto>> HandleAsync(
        BrowseDocuments query,
        CancellationToken cancellationToken = default)
    {
        var documents = await _documentsRepository.BrowseAsync();
        return documents.Select(_mapper.Map).ToList();
    }
}