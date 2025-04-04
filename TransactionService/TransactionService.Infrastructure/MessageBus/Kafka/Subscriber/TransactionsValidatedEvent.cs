using System.Text.Json.Serialization;
using TransactionService.Domain.Entities.Transaction.Commands;
using TransactionService.Domain.Entities.Transaction.Enums;
using TransactionService.Domain.Entities.Transaction.ValueObjects;

namespace TransactionService.Infrastructure.MessageBus.Kafka.Subscriber;

public class TransactionsValidatedEvent
{
    [JsonPropertyName("transactionId")]
    public Guid TransactionId { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    public UpdateTransactionCommand ToUpdateTransactionCommand()
    {
        return new UpdateTransactionCommand
        {
            TransactionId = TransactionId,
            Status = (Status)Status
        };
    }
}
