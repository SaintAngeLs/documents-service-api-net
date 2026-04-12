using Documents.Core.Events;

namespace Documents.Core.Abstractions;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> Events { get; }
    void ClearEvents();
}