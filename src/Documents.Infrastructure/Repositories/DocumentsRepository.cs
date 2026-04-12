using Documents.Core.Documents.Entities;
using Documents.Core.Documents.Repositories;
using Documents.Core.Documents.ValueObjects;
using Documents.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Documents.Infrastructure.Repositories;

public sealed class DocumentsRepository : IDocumentsRepository
{
    private readonly DocumentsDbContext _dbContext;

    public DocumentsRepository(DocumentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Document?> GetAsync(DocumentId id)
    {
        return await _dbContext.Documents
            .Include("_items")
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsAsync(DocumentId id)
    {
        return await _dbContext.Documents.AnyAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<Document>> BrowseAsync()
    {
        return await _dbContext.Documents
            .Include("_items")
            .OrderByDescending(x => x.CreatedAtUtc.Value)
            .ToListAsync();
    }

    public async Task AddAsync(Document document)
    {
        await _dbContext.Documents.AddAsync(document);
    }

    public Task UpdateAsync(Document document)
    {
        _dbContext.Documents.Update(document);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(DocumentId id)
    {
        var document = await _dbContext.Documents
            .Include("_items")
            .SingleOrDefaultAsync(x => x.Id == id);

        if (document is null)
        {
            return;
        }

        _dbContext.Documents.Remove(document);
    }
}