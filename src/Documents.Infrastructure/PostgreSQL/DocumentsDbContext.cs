using Documents.Infrastructure.PostgreSQL.Configurations;
using Documents.Infrastructure.PostgreSQL.Documents.Entities;
using Documents.Infrastructure.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Documents.Infrastructure.PostgreSQL;

public sealed class DocumentsDbContext : DbContext
{
    public DbSet<DocumentEntity> Documents => Set<DocumentEntity>();
    public DbSet<DocumentItemEntity> DocumentItems => Set<DocumentItemEntity>();
    public DbSet<IntegrationCheckpoint> IntegrationCheckpoints => Set<IntegrationCheckpoint>();

    public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentsDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}