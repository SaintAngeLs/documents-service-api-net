using Documents.Application.Abstractions;
using Documents.Application.Documents.DTO;
using Documents.Core.Documents.Repositories;

namespace Documents.Application.Documents.Queries.GetDocumentsAggregate;

public sealed class GetDocumentsAggregateHandler
{
    private readonly IDocumentsRepository _documentsRepository;
    private readonly IClock _clock;

    public GetDocumentsAggregateHandler(
        IDocumentsRepository documentsRepository,
        IClock clock)
    {
        _documentsRepository = documentsRepository;
        _clock = clock;
    }

    public async Task<DocumentsAggregateDto> HandleAsync(
        GetDocumentsAggregate query,
        CancellationToken cancellationToken = default)
    {
        var documents = await _documentsRepository.BrowseAsync();

        var rows = documents
            .Where(x => !x.IntegratedAtUtc.HasValue)
            .SelectMany(x => x.Items.Select(item => new
            {
                Kind = x.Kind.Value,
                TaxRate = item.TaxRate.Value,
                NetValue = item.NetValue.Value
            }))
            .GroupBy(x => new { x.Kind, x.TaxRate })
            .Select(x => new DocumentsAggregateRowDto
            {
                Kind = x.Key.Kind,
                TaxRate = x.Key.TaxRate,
                TotalNetValue = x.Sum(y => y.NetValue)
            })
            .OrderBy(x => x.Kind)
            .ThenBy(x => x.TaxRate)
            .ToList();

        return new DocumentsAggregateDto
        {
            GeneratedAtUtc = _clock.Now().Value,
            Rows = rows
        };
    }
}