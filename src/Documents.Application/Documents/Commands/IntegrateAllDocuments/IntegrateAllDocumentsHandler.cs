using Documents.Application.Abstractions;
using Documents.Application.Documents.DTO;
using Documents.Core.Documents.Repositories;

namespace Documents.Application.Documents.Commands.IntegrateAllDocuments;

public sealed class IntegrateAllDocumentsHandler
{
    private readonly IIntegrableDocumentsRepository _integrableDocumentsRepository;
    private readonly IDocumentsRepository _documentsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;

    public IntegrateAllDocumentsHandler(
        IIntegrableDocumentsRepository integrableDocumentsRepository,
        IDocumentsRepository documentsRepository,
        IUnitOfWork unitOfWork,
        IClock clock)
    {
        _integrableDocumentsRepository = integrableDocumentsRepository;
        _documentsRepository = documentsRepository;
        _unitOfWork = unitOfWork;
        _clock = clock;
    }

    public async Task<IntegrationResultDto> HandleAsync(
        IntegrateAllDocuments command,
        CancellationToken cancellationToken = default)
    {
        var integratedAtUtc = _clock.Now();
        var documents = await _integrableDocumentsRepository.BrowseNotIntegratedAsync();

        foreach (var document in documents)
        {
            document.MarkAsIntegrated(integratedAtUtc);
            await _documentsRepository.UpdateAsync(document);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new IntegrationResultDto
        {
            IntegratedDocumentsCount = documents.Count,
            IntegratedAtUtc = integratedAtUtc.Value,
            DocumentIds = documents.Select(x => x.Id.Value).ToList()
        };
    }
}