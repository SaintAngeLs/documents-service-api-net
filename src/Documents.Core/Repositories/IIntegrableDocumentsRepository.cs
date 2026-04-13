using Documents.Core.Documents.Entities;

namespace Documents.Core.Documents.Repositories;

public interface IIntegrableDocumentsRepository
{
    Task<IReadOnlyList<Document>> BrowseNotIntegratedAsync();
}