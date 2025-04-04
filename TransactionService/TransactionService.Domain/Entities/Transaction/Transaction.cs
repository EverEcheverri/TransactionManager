using TransactionService.Domain.Entities.Transaction.Enums;
using TransactionService.Domain.Entities.Transaction.Events;
using TransactionService.Domain.Entities.Transaction.Exceptions;
using TransactionService.Domain.Entities.Transaction.ValueObjects;
using TransactionService.Domain.SharedKernel;

namespace TransactionService.Domain.Entities.Transaction;

public sealed class Transaction : Entity
{
    private Transaction()
    {

    }

    private Transaction(Guid id, 
        AccountId sourceAccountId,
        AccountId tarjetAccountId,
        TransferType transferType,
        Amount amount,
        Status status) : base(id)
    {
        SourceAccountId = sourceAccountId;
        TarjetAccountId = tarjetAccountId;
        TransferType = transferType;
        Amount = amount;
        Status = status;
    }

    public AccountId SourceAccountId { get; private set; }
    public AccountId TarjetAccountId { get; private set; }
    public TransferType TransferType { get; private set; }
    public Amount Amount { get; private set; }
    public Status Status { get; private set; }

    public static Transaction Build(AccountId sourceAccountId, AccountId tarjetAccountId,
        TransferType transferType, Amount amount)
    {
        if (sourceAccountId.ToString().Equals(Guid.Empty.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            throw new NoValidSourceAccountIdException();
        }

        var transaction = new Transaction(Guid.NewGuid(),
            sourceAccountId,
            tarjetAccountId,
            transferType,
            amount,
            Status.Pending);
        transaction.TransactionCreatedEvent();
        return transaction;
    }

    public void UpdateStatus(Status status)
    {
        Status = status;
    }
}
