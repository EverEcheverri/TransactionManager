using AntiFraud.Worker;
using AntiFraud.Worker.Application.Features.Transaction.Validate;
using AntiFraud.Worker.Domain.Transaction;
using AntiFraud.Worker.MessageBus.Kafka.Publisher;
using AntiFraud.Worker.MessageBus.Kafka.Subscriber;
using AntiFraud.Worker.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

//Register Kafka Worker Service
builder.Services.AddHostedService<TransactionsCreatedSubscriber>();

//Register Custom Services
builder.Services.AddScoped<IValidateTransaction, ValidateTransactionUseCase>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IPublisher, Publisher>();

//Register Database Context
builder.Services.AddDbContext<AntiFraudContext>();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AntiFraudContext>();
    dbContext.Database.Migrate();
}

host.Run();
