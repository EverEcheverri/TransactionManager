using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities.Transaction.Repositories;

namespace TransactionService.Infrastructure.EntityFramework.Transaction.Repositories;

internal class TransactionRepository : ITransactionRepository
{
    private readonly TransactionContext _context;
    public TransactionRepository(TransactionContext context)
    {
        _context = context;
    }
    public async Task<Domain.Entities.Transaction.Transaction?> GetByIdAsync(Guid transactionId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _context.Transactions.FindAsync([transactionId], cancellationToken: cancellationToken);
    }

    public async Task SaveAsync(Domain.Entities.Transaction.Transaction transaction, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _context.Transactions.AddAsync(transaction, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Domain.Entities.Transaction.Transaction transaction, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _context.Entry(transaction).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
