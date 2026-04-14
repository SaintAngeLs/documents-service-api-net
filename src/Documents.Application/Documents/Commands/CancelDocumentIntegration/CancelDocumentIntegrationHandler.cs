using Documents.Application.Abstractions;
using Documents.Application.Documents.DTO;
using Documents.Application.Exceptions;
using Documents.Core.Documents.Repositories;
using Documents.Core.Documents.ValueObjects;

namespace Documents.Application.Documents.Commands.CancelDocumentIntegration;

public sealed class CancelDocumentIntegrationHandler
{
    private readonly IDocumentsRepository _documentsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelDocumentIntegrationHandler(
        IDocumentsRepository documentsRepository,
        IUnitOfWork unitOfWork)
    {
        _documentsRepository = documentsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DocumentIntegrationOperationDto> HandleAsync(
        CancelDocumentIntegration command,
        CancellationToken cancellationToken = default)
    {
        var documentId = new DocumentId(command.Id);
        var document = await _documentsRepository.GetAsync(documentId);

        if (document is null)
        {
            throw new DocumentNotFoundException(documentId);
        }

        document.CancelIntegration();
        await _documentsRepository.UpdateAsync(document);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DocumentIntegrationOperationDto
        {
            DocumentId = document.Id.Value,
            IsIntegrated = document.IntegratedAtUtc.HasValue,
            IntegratedAtUtc = document.IntegratedAtUtc.Value
        };
    }
}