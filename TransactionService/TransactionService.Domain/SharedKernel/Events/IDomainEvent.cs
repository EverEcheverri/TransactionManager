using MediatR;

namespace TransactionService.Domain.SharedKernel.Events;

public interface IDomainEvent
{
    public List<DomainEvent> DomainEvents { get; set; }
}

public abstract class DomainEvent : INotification
{
    protected DomainEvent()
    {
        OccurredAtUtc = DateTime.UtcNow;
    }
    public DateTime OccurredAtUtc { get; protected set; }
}
