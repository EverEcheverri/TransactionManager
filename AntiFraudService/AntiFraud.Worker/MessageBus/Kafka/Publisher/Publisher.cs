using AntiFraud.Worker.Domain.Transaction;
using Confluent.Kafka;
using System.Text.Json;

namespace AntiFraud.Worker.MessageBus.Kafka.Publisher;

public class Publisher : IPublisher
{
    private readonly ILogger<Publisher> _logger;
    private readonly ProducerConfig _config;
    private const int PublishTimeoutSeconds = 5; // Timeout for Kafka

    public Publisher(ILogger<Publisher> logger)
    {
        _logger = logger;
        _config = new ProducerConfig { BootstrapServers = "localhost:9092" };
    }

    public async Task PublishAsync(TransactionStatusEvent transactionStatusEvent, CancellationToken cancellationToken)
    {
        using var producer = new ProducerBuilder<string, string>(_config).Build();

        var topic = "transactions.validated";
        var key = Guid.NewGuid().ToString();

        try
        {
            string jsonValue = JsonSerializer.Serialize(transactionStatusEvent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            _logger.LogInformation($"Publishing event: {jsonValue}");

            var produceTask = producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = transactionStatusEvent.TransactionId.ToString(),
                Value = jsonValue
            });

            // Wait for either the Kafka response or a timeout
            var completedTask = await Task.WhenAny(produceTask, Task.Delay(TimeSpan.FromSeconds(PublishTimeoutSeconds), cancellationToken));

            if (completedTask == produceTask)
            {
                // If Kafka responds in time
                var deliveryReport = await produceTask;
                _logger.LogInformation($"Message sent to {deliveryReport.TopicPartitionOffset}");
            }
            else
            {
                // Timeout occurred
                _logger.LogError("Kafka produce operation timed out.");
            }

        }
        catch (OperationCanceledException)
        {
            _logger.LogError("Kafka produce operation was cancelled.");
        }
        catch (ProduceException<string, string> e)
        {
            _logger.LogError($"Kafka delivery failed: {e.Error.Reason}");
        }
    }
}
