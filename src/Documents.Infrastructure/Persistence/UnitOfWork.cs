using Documents.Application.Abstractions;

namespace Documents.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DocumentsDbContext _dbContext;

    public UnitOfWork(DocumentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _dbContext.SaveChangesAsync(cancellationToken);
}