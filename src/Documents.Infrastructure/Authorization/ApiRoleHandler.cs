using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Documents.Infrastructure.Authorization;

public sealed class ApiRoleHandler : AuthorizationHandler<ApiRoleRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiRoleHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ApiRoleRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return Task.CompletedTask;
        }

        if (!httpContext.Request.Headers.TryGetValue(ApiRoleConstants.HeaderName, out var headerValue))
        {
            return Task.CompletedTask;
        }

        if (string.Equals(headerValue.ToString(), requirement.RequiredRole, StringComparison.OrdinalIgnoreCase))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}