using Microsoft.AspNetCore.Authorization;

namespace Documents.Infrastructure.Authorization;

public sealed class ApiRoleRequirement : IAuthorizationRequirement
{
    public string RequiredRole { get; }

    public ApiRoleRequirement(string requiredRole)
    {
        RequiredRole = requiredRole;
    }
}