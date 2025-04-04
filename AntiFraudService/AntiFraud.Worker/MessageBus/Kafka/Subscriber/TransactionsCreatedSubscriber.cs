using AntiFraud.Worker.Application.Features.Transaction.Validate;
using Confluent.Kafka;
using System.Text.Json;

namespace AntiFraud.Worker.MessageBus.Kafka.Subscriber
{
    public class TransactionsCreatedSubscriber : BackgroundService
    {        
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TransactionsCreatedSubscriber> _logger;
        private readonly IConsumer<string, string> _consumer;

        public TransactionsCreatedSubscriber(ILogger<TransactionsCreatedSubscriber> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "antifraud-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe("transactions.created"); // Topic name
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Kafka Consumer...");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);

                    if (consumeResult?.Message != null)
                    {
                        _logger.LogInformation($"Received message: {consumeResult.Message.Value}");

                        var transactionEvent = JsonSerializer.Deserialize<TransactionCreatedEvent>(consumeResult.Message.Value);
                        if (transactionEvent != null)
                        {
                            using var scope = _serviceProvider.CreateScope();
                            var _validateTransaction = scope.ServiceProvider.GetServices<IValidateTransaction>().First();
                            await _validateTransaction.ExecuteAsync(transactionEvent, cancellationToken);
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
}
