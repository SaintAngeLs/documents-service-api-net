using Documents.Core.Documents.Entities;
using Documents.Core.Documents.Repositories;
using Documents.Core.Documents.ValueObjects;
using Documents.Infrastructure.PostgreSQL.Documents.Entities;
using Documents.Infrastructure.PostgreSQL.Documents.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Documents.Infrastructure.PostgreSQL.Documents.Repositories;

internal sealed class PostgresDocumentsRepository : IDocumentsRepository
{
    private readonly DocumentsDbContext _dbContext;

    public PostgresDocumentsRepository(DocumentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Document?> GetAsync(DocumentId id)
    {
        var entity = await _dbContext.Documents
            .Include(x => x.Items)
            .SingleOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : DocumentEntityMapper.ToDomain(entity);
    }

    public async Task<bool> ExistsAsync(DocumentId id)
    {
        return await _dbContext.Documents
            .AnyAsync(x => x.Id == id.Value);
    }

    public async Task<IReadOnlyList<Document>> BrowseAsync()
    {
        var entities = await _dbContext.Documents
            .Include(x => x.Items)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();

        return entities
            .Select(DocumentEntityMapper.ToDomain)
            .ToList();
    }

    public async Task AddAsync(Document document)
    {
        var entity = DocumentEntityMapper.ToEntity(document);
        await _dbContext.Documents.AddAsync(entity);
    }

    public Task UpdateAsync(Document document)
    {
        var entity = DocumentEntityMapper.ToEntity(document);
        _dbContext.Documents.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(DocumentId id)
    {
        var entity = await _dbContext.Documents
            .SingleOrDefaultAsync(x => x.Id == id.Value);

        if (entity is not null)
        {
            _dbContext.Documents.Remove(entity);
        }
    }
}