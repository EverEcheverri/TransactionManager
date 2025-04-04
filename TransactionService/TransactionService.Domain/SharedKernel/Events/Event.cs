namespace TransactionService.Domain.SharedKernel.Events;

public class Event<T> : IDomainEvent
{
    public T Body { get; set; }
    public Header Header { get; set; }
    public List<DomainEvent> DomainEvents { get; set; }
}
