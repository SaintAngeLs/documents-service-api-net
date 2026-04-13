using Documents.Application.Abstractions;
using Documents.Core.Documents.Repositories;
using Documents.Infrastructure.Authorization;
using Documents.Infrastructure.Clock;
using Documents.Infrastructure.Middleware;
using Documents.Infrastructure.PostgreSQL;
using Documents.Infrastructure.PostgreSQL.Documents.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Documents.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("postgres");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'postgres' is missing.");
        }

        services.AddDbContext<DocumentsDbContext>(options =>
            options.UseNpgsql(connectionString)
                   .UseSnakeCaseNamingConvention());

        services.AddHttpContextAccessor();

        services.AddAuthentication(ApiAuthenticationHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, ApiAuthenticationHandler>(
                ApiAuthenticationHandler.SchemeName,
                _ => { });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("LocalPolicy", policy =>
            {
                policy.AddAuthenticationSchemes(ApiAuthenticationHandler.SchemeName);
                policy.RequireAuthenticatedUser();
                policy.Requirements.Add(new ApiRoleRequirement(ApiRoleConstants.Local));
            });

            options.AddPolicy("IntegratorPolicy", policy =>
            {
                policy.AddAuthenticationSchemes(ApiAuthenticationHandler.SchemeName);
                policy.RequireAuthenticatedUser();
                policy.Requirements.Add(new ApiRoleRequirement(ApiRoleConstants.Integrator));
            });
        });

        services.AddSingleton<IAuthorizationHandler, ApiRoleHandler>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDocumentsRepository, PostgresDocumentsRepository>();
        services.AddScoped<IIntegrableDocumentsRepository, PostgresDocumentsRepository>();
        services.AddScoped<ExceptionHandlingMiddleware>();
        services.AddSingleton<IClock, UtcClock>();

        return services;
    }
}