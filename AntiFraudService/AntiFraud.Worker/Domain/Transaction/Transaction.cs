using AntiFraud.Worker.Domain.Transaction.Enums;

namespace AntiFraud.Worker.Domain.Transaction;

public sealed class Transaction
{
    private Transaction()
    {

    }

    private Transaction(Guid transactionId,
        Guid accountId,
        decimal amount,
        Status status,
        DateTime dateOccurredUtc,
        string? rejectionReason = null)
    {
        TransactionId = transactionId;
        AccountId = accountId;
        Amount = amount;
        Status = status;
        DateOccurredUtc = dateOccurredUtc;
        RejectionReason = rejectionReason;
    }

    public Guid TransactionId { get; private set; }
    public Guid AccountId { get; private set; }
    public decimal Amount { get; private set; }
    public Status Status { get; private set; }
    public DateTime DateOccurredUtc { get; private set; }
    public string? RejectionReason { get; private set; }


    public static Transaction Build(Guid transactionId, Guid accountId, decimal amount, Status status, DateTime dateOccurredUtc, string? rejectionReason = null)
    {
        var transaction =
            new Transaction(transactionId,
            accountId,
            amount,
            status,
            dateOccurredUtc,
            rejectionReason);

        return transaction;
    }

    public TransactionStatusEvent ToEvent()
    {
        return new TransactionStatusEvent(TransactionId, Status);
    }
}
