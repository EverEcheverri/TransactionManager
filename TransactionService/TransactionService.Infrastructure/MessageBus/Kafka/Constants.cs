namespace TransactionService.Infrastructure.MessageBus.Kafka;

public static class Constants
{
    public static readonly string Kafka = "kafka";
    public static readonly string BootstrapServers = "kafka:BootstrapServers";
    public static readonly string AntifraudGroupId = "kafka:AntifraudGroupId";
    public static readonly string TransactionsCreatedTopic = "Kafka:Topics:TransactionsCreated";
    public static readonly string TransactionsValidatedTopic = "Kafka:Topics:TransactionsValidated";
}

