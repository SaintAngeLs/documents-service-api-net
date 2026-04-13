using Documents.Application.Abstractions;

namespace Documents.Infrastructure.PostgreSQL;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DocumentsDbContext _dbContext;

    public UnitOfWork(DocumentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}