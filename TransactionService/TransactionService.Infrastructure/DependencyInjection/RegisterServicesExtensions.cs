using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionService.Domain.Entities.Transaction.Events;
using TransactionService.Domain.Entities.Transaction.Repositories;
using TransactionService.Infrastructure.EntityFramework.Transaction.Repositories;
using TransactionService.Infrastructure.MessageBus.Kafka;

namespace TransactionService.Infrastructure.DependencyInjection;

public static class RegisterServicesExtensions
{
    public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services
        .AddScoped<ITransactionRepository, TransactionRepository>()
        .AddScoped<IPublisher, Publisher>();
    }
}
