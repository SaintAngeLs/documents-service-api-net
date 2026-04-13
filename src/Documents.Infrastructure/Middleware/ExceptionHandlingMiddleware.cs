using Documents.Application.Exceptions;
using Documents.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;  

using ApplicationException = Documents.Application.Exceptions.ApplicationException;

namespace Documents.Infrastructure.Middleware;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (DocumentNotFoundException ex)
        {
            _logger.LogWarning(ex, "Document not found.");
            await WriteProblemDetailsAsync(context, StatusCodes.Status404NotFound, ex.Code, ex.Message);
        }
        catch (DocumentConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Document concurrency conflict.");
            await WriteProblemDetailsAsync(context, StatusCodes.Status409Conflict, ex.Code, ex.Message);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain exception.");
            await WriteProblemDetailsAsync(context, StatusCodes.Status400BadRequest, ex.Code, ex.Message);
        }
        catch (ApplicationException ex)
        {
            _logger.LogWarning(ex, "Application exception.");
            await WriteProblemDetailsAsync(context, StatusCodes.Status400BadRequest, ex.Code, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized.");
            await WriteProblemDetailsAsync(context, StatusCodes.Status401Unauthorized, "unauthorized", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception.");
            await WriteProblemDetailsAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "internal_server_error",
                "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblemDetailsAsync(
        HttpContext context,
        int statusCode,
        string title,
        string detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}