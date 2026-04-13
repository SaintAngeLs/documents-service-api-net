using Documents.Application.Abstractions;
using Documents.Application.Documents.DTO;
using Documents.Application.Documents.Services;
using Documents.Core.Documents.Entities;
using Documents.Core.Documents.Repositories;
using Documents.Core.Documents.ValueObjects;

namespace Documents.Application.Documents.Commands.CreateDocument;

public sealed class CreateDocumentHandler
{
    private readonly IDocumentsRepository _documentsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;
    private readonly IDocumentMapper _mapper;

    public CreateDocumentHandler(
        IDocumentsRepository documentsRepository,
        IUnitOfWork unitOfWork,
        IClock clock,
        IDocumentMapper mapper)
    {
        _documentsRepository = documentsRepository;
        _unitOfWork = unitOfWork;
        _clock = clock;
        _mapper = mapper;
    }

    public async Task<DocumentDetailsDto> HandleAsync(
        CreateDocument command,
        CancellationToken cancellationToken = default)
    {
        var document = Document.Create(
            new DescriptiveNo(command.DescriptiveNo),
            DocumentKind.From(command.Kind),
            _clock.Now());

        foreach (var item in command.Items)
        {
            document.AddItem(new DocumentItem(
                DocumentItemId.Create(),
                document.Id,
                new ArticleName(item.ArticleName),
                new TaxRate(item.TaxRate),
                new NetValue(item.NetValue)));
        }

        document.ValidateBeforeSave();

        await _documentsRepository.AddAsync(document);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.MapDetails(document);
    }
}