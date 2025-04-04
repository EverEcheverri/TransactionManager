using TransactionService.Domain.Entities.Transaction.Commands;

namespace TransactionService.Application.Transaction.Interfaces;

public interface IUpdateTransactionStatus
{
    Task ExecuteAsync(UpdateTransactionCommand updateTransactionCommand, CancellationToken cancellationToken);
}
