using Microsoft.AspNetCore.Mvc;

namespace Documents.Api.Controllers;

/// <summary>
/// Endpoint startowy aplikacji.
/// </summary>
[ApiController]
[Route("/")]
public sealed class HomeController : ControllerBase
{
    /// <summary>
    /// Zwraca podstawową informację o API.
    /// </summary>
    /// <response code="200">Zwrócono komunikat powitalny.</response>
    [HttpGet]
    [EndpointSummary("Strona główna API.")]
    [EndpointDescription("Zwraca prosty komunikat potwierdzający, że aplikacja działa.")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok("Witaj w Documents API!");
    }
}