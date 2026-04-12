using Documents.Core.Events;

namespace Documents.Core.Abstractions;

public abstract class AggregateRoot : IAggregateRoot
{
    private readonly List<IDomainEvent> _events = new();

    public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

    protected void AddEvent(IDomainEvent domainEvent)
        => _events.Add(domainEvent);

    public void ClearEvents()
        => _events.Clear();
}