using TransactionService.Domain.Entities.Transaction.Commands;

namespace TransactionService.Application.Transaction.Interfaces;

public interface ICreateTransaction
{
    Task<Guid> ExecuteAsync(CreateTransactionCommand createTransactionCommand, CancellationToken cancellationToken);
}
