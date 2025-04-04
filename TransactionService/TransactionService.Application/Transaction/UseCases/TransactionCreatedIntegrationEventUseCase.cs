using MediatR;
using TransactionService.Domain.Entities.Transaction.Events;
using TransactionService.Domain.SharedKernel.Events;

namespace TransactionService.Application.Transaction.UseCases;

public class TransactionCreatedIntegrationEventUseCase : INotificationHandler<TransactionCreated>
{
    private readonly Domain.Entities.Transaction.Events.IPublisher _publisher;

    public TransactionCreatedIntegrationEventUseCase(Domain.Entities.Transaction.Events.IPublisher publisher)
    {
        _publisher = publisher;
    }

    private const string CreatedSubject = "Created";
    public async Task Handle(TransactionCreated transactionCreated, CancellationToken cancellationToken)
    {
        var transactionCreatedEvent = new Event<TransactionCreated>
        {
            Header = new Header
            {
                Key = transactionCreated.TransactionId,
                EventType = nameof(TransactionCreated),
                Subject = CreatedSubject
            },
            Body = new TransactionCreated
            {
                TransactionId = transactionCreated.TransactionId,
                SourceAccountId = transactionCreated.SourceAccountId,
                TarjetAccountId = transactionCreated.TarjetAccountId,
                TransferType = transactionCreated.TransferType,
                Amount = transactionCreated.Amount,
                Status = transactionCreated.Status
            },
        };

        var eventToPublish = new List<IDomainEvent> { transactionCreatedEvent };

        await _publisher.PublishAsync(eventToPublish, cancellationToken);
    }
}