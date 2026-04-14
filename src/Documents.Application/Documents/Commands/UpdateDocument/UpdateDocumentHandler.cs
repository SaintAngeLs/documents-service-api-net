using Documents.Application.Abstractions;
using Documents.Application.Documents.DTO;
using Documents.Application.Documents.Services;
using Documents.Application.Exceptions;
using Documents.Core.Documents.Entities;
using Documents.Core.Documents.Repositories;
using Documents.Core.Documents.ValueObjects;

namespace Documents.Application.Documents.Commands.UpdateDocument;

public sealed class UpdateDocumentHandler
{
    private readonly IDocumentsRepository _documentsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDocumentMapper _mapper;

    public UpdateDocumentHandler(
        IDocumentsRepository documentsRepository,
        IUnitOfWork unitOfWork,
        IDocumentMapper mapper)
    {
        _documentsRepository = documentsRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DocumentDetailsDto> HandleAsync(
        UpdateDocument command,
        CancellationToken cancellationToken = default)
    {
        var documentId = new DocumentId(command.Id);
        var document = await _documentsRepository.GetAsync(documentId);

        if (document is null)
        {
            throw new DocumentNotFoundException(documentId);
        }

        if (document.Revision.Value != command.Revision)
        {
            throw new DocumentConcurrencyException(documentId);
        }

        document.ChangeDescriptiveNo(new DescriptiveNo(command.DescriptiveNo));
        document.ChangeKind(DocumentKind.From(command.Kind));

        var existingItems = document.Items.ToDictionary(x => x.Id.Value, x => x);

        foreach (var existingItem in document.Items.ToList())
        {
            var stillExists = command.Items.Any(x => x.Id == existingItem.Id.Value);
            if (!stillExists)
            {
                document.RemoveItem(existingItem.Id);
            }
        }

        foreach (var item in command.Items)
        {
            var itemId = item.Id == Guid.Empty
                ? DocumentItemId.Create()
                : new DocumentItemId(item.Id);

            if (!existingItems.ContainsKey(itemId.Value))
            {
                document.AddItem(new DocumentItem(
                    itemId,
                    document.Id,
                    new ArticleName(item.ArticleName),
                    new TaxRate(item.TaxRate),
                    new NetValue(item.NetValue)));

                continue;
            }

            document.UpdateItem(
                itemId,
                new ArticleName(item.ArticleName),
                new TaxRate(item.TaxRate),
                new NetValue(item.NetValue));
        }

        document.ValidateBeforeSave();
        document.IncreaseRevision();

        await _documentsRepository.UpdateAsync(document);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.MapDetails(document);
    }
}