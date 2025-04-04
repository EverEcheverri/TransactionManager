using TransactionService.Domain.Entities.Transaction.ValueObjects;

namespace TransactionService.Domain.Entities.Transaction.Repositories;

public interface ITransactionRepository
{
    Task SaveAsync(Transaction transaction, CancellationToken cancellationToken);
    Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken);
    Task<Transaction?> GetByIdAsync(Guid transactionId, CancellationToken cancellationToken);
}
