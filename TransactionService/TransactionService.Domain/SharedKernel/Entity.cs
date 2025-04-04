using TransactionService.Domain.SharedKernel.Events;

namespace TransactionService.Domain.SharedKernel;

public class Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;
    protected Entity()
    {
    }
    protected Entity(Guid id)
      : this()
    {
        Id = id;
    }

    public Guid Id { get; }
    internal void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}