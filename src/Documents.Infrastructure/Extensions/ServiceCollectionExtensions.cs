using Documents.Application.Abstractions;
using Documents.Core.Documents.Repositories;
using Documents.Infrastructure.Authorization;
using Documents.Infrastructure.Clock;
using Documents.Infrastructure.Middleware;
using Documents.Infrastructure.Persistence;
using Documents.Infrastructure.Repositories;
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
        {
            options.UseNpgsql(connectionString);
            options.UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IDocumentsRepository, DocumentsRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IClock, UtcClock>();

        services.AddHttpContextAccessor();
        services.AddTransient<ExceptionHandlingMiddleware>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(ApiRoleConstants.LocalPolicy, policy =>
            {
                policy.Requirements.Add(new ApiRoleRequirement(ApiRoleConstants.Local));
            });

            options.AddPolicy(ApiRoleConstants.IntegratorPolicy, policy =>
            {
                policy.Requirements.Add(new ApiRoleRequirement(ApiRoleConstants.Integrator));
            });
        });

        services.AddSingleton<IAuthorizationHandler, ApiRoleHandler>();

        return services;
    }
}