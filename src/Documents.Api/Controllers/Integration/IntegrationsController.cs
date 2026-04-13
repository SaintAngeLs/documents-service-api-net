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
        return Ok(result);
    }
}