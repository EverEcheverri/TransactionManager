using TransactionService.Domain.SharedKernel.Events;

namespace TransactionService.Domain.Entities.Transaction.Events;

public interface IPublisher
{
    Task PublishAsync(IReadOnlyList<IDomainEvent> events, CancellationToken cancellationToken);
}
