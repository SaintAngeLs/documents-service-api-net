using Documents.Core.Documents.Entities;
using Documents.Core.Documents.ValueObjects;

namespace Documents.Core.Documents.Repositories;

public interface IDocumentsRepository
{
    Task<Document?> GetAsync(DocumentId id);
    Task<bool> ExistsAsync(DocumentId id);
    Task<IReadOnlyList<Document>> BrowseAsync();
    Task AddAsync(Document document);
    Task UpdateAsync(Document document);
    Task DeleteAsync(DocumentId id);
}