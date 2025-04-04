using AntiFraud.Worker.Domain.Transaction;
using AntiFraud.Worker.Domain.Transaction.Enums;
using AntiFraud.Worker.MessageBus.Kafka.Publisher;
using AntiFraud.Worker.MessageBus.Kafka.Subscriber;
using System.Text;

namespace AntiFraud.Worker.Application.Features.Transaction.Validate;

public class ValidateTransactionUseCase : IValidateTransaction
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPublisher _publisher;
    private const decimal MaxSingleTransactionAmount = 2000;
    private const decimal MaxTotalAmountPerDay = 20000;

    public ValidateTransactionUseCase(ITransactionRepository transactionRepository, IPublisher publisher)
    {
        _transactionRepository = transactionRepository;
        _publisher = publisher;
    }

    public async Task ExecuteAsync(TransactionCreatedEvent @event, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var today = DateTime.UtcNow;

        // Validate the transaction
        var (status, rejectionReason) = await ValidateTransactionAsync(@event, today, cancellationToken);

        var transaction = Domain.Transaction.Transaction.Build(
            @event.TransactionId,
            @event.SourceAccountId,
            @event.Amount,
            status,
            @event.OccurredAtUtc,
            rejectionReason
        );

        await _transactionRepository.SaveAsync(transaction, cancellationToken);
        await _publisher.PublishAsync(transaction.ToEvent(), cancellationToken);
    }

    private async Task<(Status status, string? rejectionReason)> ValidateTransactionAsync(
    TransactionCreatedEvent @event,
    DateTime today,
    CancellationToken cancellationToken)
    {
        var rejectionReasons = new List<string>();
        Status status = Status.Approved;

        if (@event.Amount > MaxSingleTransactionAmount)
        {
            status = Status.Rejected;
            rejectionReasons.Add($"Transaction amount cannot exceed: {MaxSingleTransactionAmount}");
        }

        var totalAmountForDay = await _transactionRepository.GetTotalAmountForDayAsync(
            @event.SourceAccountId, today, cancellationToken);

        if (totalAmountForDay + @event.Amount > MaxTotalAmountPerDay)
        {
            status = Status.Rejected;
            rejectionReasons.Add($"Total transactions for the day cannot exceed {MaxTotalAmountPerDay}");
        }

        return (status, rejectionReasons.Count > 0 ? string.Join(", ", rejectionReasons) : null);
    }

}
