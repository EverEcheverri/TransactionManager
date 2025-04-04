using AntiFraud.Worker.MessageBus.Kafka.Subscriber;

namespace AntiFraud.Worker.Application.Features.Transaction.Validate;

public interface IValidateTransaction
{
    Task ExecuteAsync(TransactionCreatedEvent transactionCreatedEvent, CancellationToken cancellationToken);
}
