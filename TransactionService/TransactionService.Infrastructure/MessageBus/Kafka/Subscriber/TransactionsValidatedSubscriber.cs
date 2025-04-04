using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TransactionService.Application.Transaction.Interfaces;

namespace TransactionService.Infrastructure.MessageBus.Kafka.Subscriber;

public class TransactionsValidatedSubscriber : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TransactionsValidatedSubscriber> _logger;
    private readonly IConsumer<string, string> _consumer;

    public TransactionsValidatedSubscriber(ILogger<TransactionsValidatedSubscriber> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
    {
        _logger = logger;

        var bootstrapServers = configuration.GetValue<string>(Constants.BootstrapServers);
        var transactionsValidatedEventsTopic = configuration.GetValue<string>(Constants.TransactionsValidatedTopic);
        var antifraudGroupId = configuration.GetValue<string>(Constants.AntifraudGroupId);

        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = antifraudGroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(transactionsValidatedEventsTopic);
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Kafka Consumer...");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(5));

                if (consumeResult?.Message != null)
                {
                    _logger.LogInformation($"Received message: {consumeResult.Message.Value}");

                    var transactionEvent = JsonSerializer.Deserialize<TransactionsValidatedEvent>(consumeResult.Message.Value);
                    if (transactionEvent != null)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var _validateTransaction = scope.ServiceProvider.GetServices<IUpdateTransactionStatus>().First();
                        await _validateTransaction.ExecuteAsync(transactionEvent.ToUpdateTransactionCommand(), cancellationToken);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Kafka message");
            }

            await Task.Delay(100, cancellationToken);
        }

        _consumer.Close();
    }
}
