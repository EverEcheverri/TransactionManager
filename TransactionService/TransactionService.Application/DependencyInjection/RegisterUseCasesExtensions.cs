using Microsoft.Extensions.DependencyInjection;
using TransactionService.Application.Transaction.Interfaces;
using TransactionService.Application.Transaction.UseCases;

namespace TransactionService.Application.DependencyInjection;

public static class RegisterUseCasesExtensions
{
    public static void AddUseCases(this IServiceCollection services)
    {
        services
            .AddScoped<ICreateTransaction, CreateTransactionUseCase>()
            .AddScoped<IUpdateTransactionStatus, UpdateTransactionStatusUseCase>();
    }

    public static void AddEventHandlersUseCases(this IServiceCollection services)
    {
        services.AddMediatR(mtr =>
        {
            mtr.RegisterServicesFromAssembly(typeof(TransactionCreatedIntegrationEventUseCase).Assembly);
        });
    }
}
