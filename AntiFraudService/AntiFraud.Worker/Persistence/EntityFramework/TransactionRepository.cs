using AntiFraud.Worker.Domain.Transaction;
using Microsoft.EntityFrameworkCore;

namespace AntiFraud.Worker.Persistence.EntityFramework;

internal class TransactionRepository : ITransactionRepository
{
    private readonly AntiFraudContext _context;
    public TransactionRepository(AntiFraudContext context)
    {
        _context = context;
    }
    public async Task<decimal> GetTotalAmountForDayAsync(Guid accountId, DateTime date, CancellationToken cancellationToken)
    {
        var transactions = await _context.Transactions
            .Where(t => t.AccountId == accountId)
            .ToListAsync(cancellationToken);

        var totalAmount = transactions
            .Where(t => t.DateOccurredUtc.Date == date.Date)
            .Sum(t => t.Amount);

        return totalAmount;
    }

    public async Task SaveAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _context.Transactions.AddAsync(transaction, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
