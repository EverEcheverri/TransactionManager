using System.Text.Json.Serialization;

namespace AntiFraud.Worker.MessageBus.Kafka.Subscriber;

public class TransactionCreatedEvent
{
    [JsonPropertyName("transactionId")]
    public Guid TransactionId { get; set; }

    [JsonPropertyName("sourceAccountId")]
    public Guid SourceAccountId { get; set; }

    [JsonPropertyName("tarjetAccountId")]
    public Guid TarjetAccountId { get; set; }

    [JsonPropertyName("transferType")]
    public int TransferType { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("occurredAtUtc")]
    public DateTime OccurredAtUtc { get; set; }
}