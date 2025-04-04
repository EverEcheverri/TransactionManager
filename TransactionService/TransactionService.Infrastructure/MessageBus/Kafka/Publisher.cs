using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TransactionService.Domain.SharedKernel.Events;

namespace TransactionService.Infrastructure.MessageBus.Kafka;

public class Publisher : Domain.Entities.Transaction.Events.IPublisher
{
    private readonly ILogger<Publisher> _logger;
    private readonly ProducerConfig _config;
    private const int PublishTimeoutSeconds = 5;
    private readonly string _transactionsEventsTopic;

    public Publisher(ILogger<Publisher> logger, IConfiguration configuration)
    {
        _logger = logger;

        var bootstrapServers = configuration.GetValue<string>(Constants.BootstrapServers);
        var transactionsEventsTopic = configuration.GetValue<string>(Constants.TransactionsCreatedTopic);

        _config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _transactionsEventsTopic = configuration.GetSection(Constants.TransactionsCreatedTopic).Value;
    }

    public async Task PublishAsync(IReadOnlyList<IDomainEvent> events, CancellationToken cancellationToken)
    {
        using var producer = new ProducerBuilder<string, string>(_config).Build();
        //var topic = "transactions.created";

        foreach (var @event in events)
        {
            try
            {
                var eventTypeClass = @event.GetType();
                if (eventTypeClass.IsGenericType && eventTypeClass.GetGenericTypeDefinition() == typeof(Event<>))
                {
                    var header = eventTypeClass?.GetProperty("Header")?.GetValue(@event)
                        as Domain.SharedKernel.Events.Header;
                    var eventData = eventTypeClass?.GetProperty("Body")?.GetValue(@event);
                    _logger.LogInformation($"Event type: {eventData.GetType()}");

                    if (eventData != null)
                    {
                        string jsonValue = JsonSerializer.Serialize(eventData, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                        _logger.LogInformation($"Publishing event: {jsonValue}");

                        var produceTask = producer.ProduceAsync(_transactionsEventsTopic, new Message<string, string>
                        {
                            Key = header?.Key.ToString(),
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
}
