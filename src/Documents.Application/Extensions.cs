using Documents.Application.Documents.Commands.CreateDocument;
using Documents.Application.Documents.Commands.UpdateDocument;
using Documents.Application.Documents.Queries.BrowseDocuments;
using Documents.Application.Documents.Queries.GetDocument;
using Documents.Application.Documents.Queries.GetDocumentIntegrationStatus;
using Documents.Application.Documents.Queries.GetDocumentsAggregate;
using Documents.Application.Documents.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Documents.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IDocumentMapper, DocumentMapper>();

        services.AddScoped<CreateDocumentHandler>();
        services.AddScoped<UpdateDocumentHandler>();

        services.AddScoped<BrowseDocumentsHandler>();
        services.AddScoped<GetDocumentHandler>();
        services.AddScoped<GetDocumentIntegrationStatusHandler>();
        services.AddScoped<GetDocumentsAggregateHandler>();

        return services;
    }
}