using Microsoft.AspNetCore.Mvc;

namespace Documents.Api.Controllers;

/// <summary>
/// Endpoint pomocniczy do dokumentacji API.
/// </summary>
[ApiController]
[Route("docs")]
public sealed class DocsController : ControllerBase
{
    /// <summary>
    /// Przekierowuje do dokumentacji Scalar.
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        return Redirect("/scalar");
    }
}