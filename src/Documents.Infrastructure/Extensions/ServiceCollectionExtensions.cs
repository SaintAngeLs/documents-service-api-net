using Documents.Application.Abstractions;
using Documents.Core.Documents.Repositories;
using Documents.Infrastructure.Clock;
using Documents.Infrastructure.PostgreSQL;
using Documents.Infrastructure.PostgreSQL.Documents.Repositories;
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

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDocumentsRepository, PostgresDocumentsRepository>();
        services.AddSingleton<IClock, UtcClock>();

        return services;
    }
}