using Documents.Core.Documents.Entities;
using Documents.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Documents.Infrastructure.Persistence;

public sealed class DocumentsDbContext : DbContext
{
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentItem> DocumentItems => Set<DocumentItem>();
    public DbSet<IntegrationCheckpoint> IntegrationCheckpoints => Set<IntegrationCheckpoint>();

    public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("documents");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentsDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}