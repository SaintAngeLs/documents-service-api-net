using Documents.Application.Abstractions;
using Documents.Application.Documents.DTO;
using Documents.Application.Exceptions;
using Documents.Core.Documents.Repositories;
using Documents.Core.Documents.ValueObjects;

namespace Documents.Application.Documents.Commands.IntegrateDocument;

public sealed class IntegrateDocumentHandler
{
    private readonly IDocumentsRepository _documentsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;

    public IntegrateDocumentHandler(
        IDocumentsRepository documentsRepository,
        IUnitOfWork unitOfWork,
        IClock clock)
    {
        _documentsRepository = documentsRepository;
        _unitOfWork = unitOfWork;
        _clock = clock;
    }

    public async Task<DocumentIntegrationOperationDto> HandleAsync(
        IntegrateDocument command,
        CancellationToken cancellationToken = default)
    {
        var documentId = new DocumentId(command.Id);
        var document = await _documentsRepository.GetAsync(documentId);

        if (document is null)
        {
            throw new DocumentNotFoundException(documentId);
        }

        if (!document.IntegratedAtUtc.HasValue)
        {
            document.MarkAsIntegrated(_clock.Now());
            await _documentsRepository.UpdateAsync(document);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new DocumentIntegrationOperationDto
        {
            DocumentId = document.Id.Value,
            IsIntegrated = document.IntegratedAtUtc.HasValue,
            IntegratedAtUtc = document.IntegratedAtUtc.Value
        };
    }
}