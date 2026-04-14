using Documents.Api.Contracts.Integration;
using Documents.Application.Documents.Commands.CancelDocumentIntegration;
using Documents.Application.Documents.Commands.IntegrateAllDocuments;
using Documents.Application.Documents.Commands.IntegrateDocument;
using Documents.Application.Documents.Queries.GetDocumentsAggregate;
using Documents.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Documents.Api.Controllers.Integration;

/// <summary>
/// Endpointy przeznaczone dla integratora.
/// </summary>
[ApiController]
[Route("api/integration/documents")]
[Authorize(Policy = ApiRoleConstants.IntegratorPolicy)]
[Produces("application/json")]
public sealed class IntegrationsController : ControllerBase
{
    /// <summary>
    /// Pobiera zagregowane dane dokumentów oczekujących na integrację.
    /// </summary>
    /// <remarks>
    /// Endpoint zwraca sumę wartości <c>NetValue</c> w podziale na:
    /// <list type="bullet">
    /// <item><description>typ dokumentu (<c>Kind</c>)</description></item>
    /// <item><description>stawkę podatku (<c>TaxRate</c>)</description></item>
    /// </list>
    /// Dane powinny obejmować dokumenty nieuwzględnione jeszcze przez integratora.
    /// </remarks>
    /// <response code="200">Zwrócono poprawnie wyliczoną agregację dokumentów.</response>
    /// <response code="400">Żądanie nie mogło zostać obsłużone z powodu błędu biznesowego.</response>
    /// <response code="403">Wywołujący nie ma uprawnień do endpointów integracyjnych.</response>
    [HttpGet("aggregates")]
    [EndpointSummary("Pobiera agregaty dokumentów dla integratora.")]
    [EndpointDescription("Zwraca sumę wartości netto w podziale na typ dokumentu i stawkę podatku dla dokumentów oczekujących na integrację.")]
    [ProducesResponseType(typeof(GetDocumentsAggregateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAggregates(
        [FromServices] GetDocumentsAggregateHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetDocumentsAggregate(), cancellationToken);

        var response = new GetDocumentsAggregateResponse
        {
            GeneratedAtUtc = result.GeneratedAtUtc,
            Rows = result.Rows.Select(x => new DocumentAggregateRowResponse
            {
                Kind = x.Kind,
                TaxRate = x.TaxRate,
                TotalNetValue = x.TotalNetValue
            }).ToList()
        };

        return Ok(response);
    }

    /// <summary>
    /// Oznacza wszystkie oczekujące dokumenty jako zintegrowane.
    /// </summary>
    /// <remarks>
    /// Endpoint wykonuje operację zbiorczą i zwraca podsumowanie:
    /// liczbę zintegrowanych dokumentów, czas operacji oraz identyfikatory dokumentów.
    /// </remarks>
    /// <response code="200">Dokumenty zostały oznaczone jako zintegrowane.</response>
    /// <response code="400">Żądanie nie mogło zostać obsłużone z powodu błędu biznesowego.</response>
    /// <response code="403">Wywołujący nie ma uprawnień do endpointów integracyjnych.</response>
    [HttpPost("integrate-all")]
    [EndpointSummary("Oznacza wszystkie oczekujące dokumenty jako zintegrowane.")]
    [EndpointDescription("Wykonuje zbiorczą operację integracji dla wszystkich dokumentów oczekujących na integrację.")]
    [ProducesResponseType(typeof(IntegrateAllDocumentsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> IntegrateAll(
        [FromServices] IntegrateAllDocumentsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new IntegrateAllDocuments(), cancellationToken);

        var response = new IntegrateAllDocumentsResponse
        {
            IntegratedDocumentsCount = result.IntegratedDocumentsCount,
            IntegratedAtUtc = result.IntegratedAtUtc,
            DocumentIds = result.DocumentIds
        };

        return Ok(response);
    }

    /// <summary>
    /// Oznacza pojedynczy dokument jako zintegrowany.
    /// </summary>
    /// <param name="id">Identyfikator dokumentu.</param>
    /// <response code="200">Dokument został oznaczony jako zintegrowany.</response>
    /// <response code="400">Żądanie nie mogło zostać obsłużone z powodu błędu biznesowego.</response>
    /// <response code="403">Wywołujący nie ma uprawnień do endpointów integracyjnych.</response>
    /// <response code="404">Nie znaleziono dokumentu o wskazanym identyfikatorze.</response>
    [HttpPost("{id:guid}/integrate")]
    [EndpointSummary("Oznacza pojedynczy dokument jako zintegrowany.")]
    [EndpointDescription("Ustawia status integracji dla wskazanego dokumentu i blokuje jego dalszą edycję.")]
    [ProducesResponseType(typeof(IntegrateDocumentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> IntegrateSingle(
        Guid id,
        [FromServices] IntegrateDocumentHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new IntegrateDocument
        {
            Id = id
        }, cancellationToken);

        var response = new IntegrateDocumentResponse
        {
            DocumentId = result.DocumentId,
            IsIntegrated = result.IsIntegrated,
            IntegratedAtUtc = result.IntegratedAtUtc
        };

        return Ok(response);
    }

    /// <summary>
    /// Cofa stan integracji pojedynczego dokumentu.
    /// </summary>
    /// <param name="id">Identyfikator dokumentu.</param>
    /// <remarks>
    /// Endpoint pomocniczy. Może być przydatny w środowisku developerskim, testowym
    /// lub w kontrolowanych scenariuszach administracyjnych.
    /// </remarks>
    /// <response code="200">Stan integracji dokumentu został cofnięty.</response>
    /// <response code="400">Żądanie nie mogło zostać obsłużone z powodu błędu biznesowego.</response>
    /// <response code="403">Wywołujący nie ma uprawnień do endpointów integracyjnych.</response>
    /// <response code="404">Nie znaleziono dokumentu o wskazanym identyfikatorze.</response>
    [HttpDelete("{id:guid}/integration")]
    [EndpointSummary("Cofa stan integracji dokumentu.")]
    [EndpointDescription("Usuwa informację o integracji wskazanego dokumentu.")]
    [ProducesResponseType(typeof(CancelDocumentIntegrationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelIntegration(
        Guid id,
        [FromServices] CancelDocumentIntegrationHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new CancelDocumentIntegration
        {
            Id = id
        }, cancellationToken);

        var response = new CancelDocumentIntegrationResponse
        {
            DocumentId = result.DocumentId,
            IsIntegrated = result.IsIntegrated,
            IntegratedAtUtc = result.IntegratedAtUtc
        };

        return Ok(response);
    }
}