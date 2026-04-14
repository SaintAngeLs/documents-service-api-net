using Documents.Api.Contracts.Local;
using Documents.Application.Documents.Commands.CreateDocument;
using Documents.Application.Documents.Commands.UpdateDocument;
using Documents.Application.Documents.Queries.BrowseDocuments;
using Documents.Application.Documents.Queries.GetDocument;
using Documents.Application.Documents.Queries.GetDocumentIntegrationStatus;
using Documents.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Documents.Api.Controllers.Local;

/// <summary>
/// Endpointy przeznaczone dla lokalnej aplikacji klienckiej.
/// </summary>
[ApiController]
[Route("api/local/documents")]
[Authorize(Policy = ApiRoleConstants.LocalPolicy)]
[Produces("application/json")]
public sealed class DocumentsController : ControllerBase
{
    /// <summary>
    /// Pobiera listę dokumentów.
    /// </summary>
    /// <remarks>
    /// Zwraca podstawowe dane dokumentów potrzebne do prezentacji listy,
    /// takie jak numer opisowy, typ dokumentu, data utworzenia i status integracji.
    /// </remarks>
    /// <response code="200">Lista dokumentów została zwrócona poprawnie.</response>
    /// <response code="403">Wywołujący nie ma uprawnień do endpointów lokalnych.</response>
    [HttpGet]
    [EndpointSummary("Pobiera listę dokumentów.")]
    [EndpointDescription("Zwraca listę dokumentów dostępnych dla lokalnej aplikacji klienckiej.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Browse(
        [FromServices] BrowseDocumentsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new BrowseDocuments(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Pobiera szczegóły pojedynczego dokumentu.
    /// </summary>
    /// <param name="id">Identyfikator dokumentu.</param>
    /// <response code="200">Szczegóły dokumentu zostały zwrócone poprawnie.</response>
    /// <response code="403">Wywołujący nie ma uprawnień do endpointów lokalnych.</response>
    /// <response code="404">Nie znaleziono dokumentu o wskazanym identyfikatorze.</response>
    [HttpGet("{id:guid}")]
    [EndpointSummary("Pobiera szczegóły dokumentu.")]
    [EndpointDescription("Zwraca pełne dane dokumentu wraz z jego pozycjami.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        Guid id,
        [FromServices] GetDocumentHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetDocument
        {
            Id = id
        }, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Pobiera status integracji dokumentu.
    /// </summary>
    /// <param name="id">Identyfikator dokumentu.</param>
    /// <response code="200">Status integracji został zwrócony poprawnie.</response>
    /// <response code="403">Wywołujący nie ma uprawnień do endpointów lokalnych.</response>
    /// <response code="404">Nie znaleziono dokumentu o wskazanym identyfikatorze.</response>
    [HttpGet("{id:guid}/integration-status")]
    [EndpointSummary("Pobiera status integracji dokumentu.")]
    [EndpointDescription("Zwraca informację, czy dokument został już uwzględniony przez integratora oraz kiedy to nastąpiło.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIntegrationStatus(
        Guid id,
        [FromServices] GetDocumentIntegrationStatusHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetDocumentIntegrationStatus
        {
            Id = id
        }, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Tworzy nowy dokument.
    /// </summary>
    /// <param name="request">Dane tworzonego dokumentu.</param>
    /// <response code="201">Dokument został utworzony poprawnie.</response>
    /// <response code="400">Żądanie nie mogło zostać obsłużone z powodu błędu walidacji lub reguł biznesowych.</response>
    /// <response code="403">Wywołujący nie ma uprawnień do endpointów lokalnych.</response>
    [HttpPost]
    [Consumes("application/json")]
    [EndpointSummary("Tworzy nowy dokument.")]
    [EndpointDescription("Tworzy dokument wraz z pozycjami. Dokument musi zawierać co najmniej jedną pozycję.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create(
        [FromBody] CreateDocumentRequest request,
        [FromServices] CreateDocumentHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateDocument
        {
            DescriptiveNo = request.DescriptiveNo,
            Kind = request.Kind,
            Items = request.Items.Select(x => new CreateDocumentItemDto
            {
                ArticleName = x.ArticleName,
                TaxRate = x.TaxRate,
                NetValue = x.NetValue
            }).ToList()
        };

        var result = await handler.HandleAsync(command, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    /// <summary>
    /// Aktualizuje istniejący dokument.
    /// </summary>
    /// <param name="id">Identyfikator dokumentu.</param>
    /// <param name="request">Nowy stan dokumentu i jego pozycji.</param>
    /// <remarks>
    /// Endpoint wykorzystuje mechanizm optimistic concurrency przez pole <c>Revision</c>.
    /// Zapis powinien zakończyć się błędem, jeśli dokument został zmieniony równolegle
    /// albo został już uwzględniony przez integratora.
    /// </remarks>
    /// <response code="200">Dokument został zaktualizowany poprawnie.</response>
    /// <response code="400">Żądanie nie mogło zostać obsłużone z powodu błędu walidacji lub reguł biznesowych.</response>
    /// <response code="403">Wywołujący nie ma uprawnień do endpointów lokalnych.</response>
    /// <response code="404">Nie znaleziono dokumentu o wskazanym identyfikatorze.</response>
    /// <response code="409">Wystąpił konflikt współbieżności podczas zapisu.</response>
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [EndpointSummary("Aktualizuje istniejący dokument.")]
    [EndpointDescription("Aktualizuje dokument i jego pozycje z użyciem revision do kontroli współbieżności.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateDocumentRequest request,
        [FromServices] UpdateDocumentHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateDocument
        {
            Id = id,
            DescriptiveNo = request.DescriptiveNo,
            Kind = request.Kind,
            Revision = request.Revision,
            Items = request.Items.Select(x => new UpdateDocumentItemDto
            {
                Id = x.Id,
                ArticleName = x.ArticleName,
                TaxRate = x.TaxRate,
                NetValue = x.NetValue
            }).ToList()
        };

        var result = await handler.HandleAsync(command, cancellationToken);

        return Ok(result);
    }
}