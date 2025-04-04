using TransactionService.Domain.SharedKernel.Events;

namespace TransactionService.Domain.Entities.Transaction.Events;

internal static class TransactionRaiseDomainEvent
{
    internal static void TransactionCreatedEvent(this Transaction transaction)
    {
        transaction.RaiseDomainEvent(new Event<TransactionCreated>
        {
            Body = new TransactionCreated
            {
                TransactionId = transaction.Id,
                SourceAccountId = transaction.SourceAccountId.Value,
                TarjetAccountId = transaction.TarjetAccountId.Value,
                TransferType = (int)transaction.TransferType,
                Amount = transaction.Amount.Value,
                Status = (int)transaction.Status
            }
        });
    }
}
