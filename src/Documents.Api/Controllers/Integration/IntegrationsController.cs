using Documents.Api.Contracts.Integration;
using Documents.Application.Documents.Commands.CancelDocumentIntegration;
using Documents.Application.Documents.Commands.IntegrateAllDocuments;
using Documents.Application.Documents.Commands.IntegrateDocument;
using Documents.Application.Documents.Queries.GetDocumentsAggregate;
using Documents.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Documents.Api.Controllers.Integration;

[ApiController]
[Route("api/integration/documents")]
[Authorize(Policy = ApiRoleConstants.IntegratorPolicy)]
public sealed class IntegrationsController : ControllerBase
{
    [HttpGet("aggregates")]
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

    [HttpPost("integrate-all")]
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

    [HttpPost("{id:guid}/integrate")]
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

    [HttpDelete("{id:guid}/integration")]
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