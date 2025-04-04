using AntiFraud.Worker.Domain.Transaction;

namespace AntiFraud.Worker.MessageBus.Kafka.Publisher;

public interface IPublisher
{
    Task PublishAsync(TransactionStatusEvent transactionStatusEvent, CancellationToken cancellationToken);
}
