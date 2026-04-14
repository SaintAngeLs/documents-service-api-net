using Documents.Api.Contracts.Local;
using Documents.Application.Documents.Commands.CreateDocument;
using Documents.Application.Documents.Commands.UpdateDocument;
using Documents.Application.Documents.Queries.BrowseDocuments;
using Documents.Application.Documents.Queries.GetDocument;
using Documents.Application.Documents.Queries.GetDocumentIntegrationStatus;
using Documents.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Documents.Api.Controllers.Local;

[ApiController]
[Route("api/local/documents")]
[Authorize(Policy = ApiRoleConstants.LocalPolicy)]
public sealed class DocumentsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Browse(
        [FromServices] BrowseDocumentsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new BrowseDocuments(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
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

    [HttpGet("{id:guid}/integration-status")]
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

    [HttpPost]
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

    [HttpPut("{id:guid}")]
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