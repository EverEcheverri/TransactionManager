namespace AntiFraud.Worker.Domain.Transaction;

public interface ITransactionRepository
{
    Task SaveAsync(Transaction transaction, CancellationToken cancellationToken);
    Task<decimal> GetTotalAmountForDayAsync(Guid accountId, DateTime date, CancellationToken cancellationToken);
}
