using AntiFraud.Worker.Domain.Transaction.Enums;

namespace AntiFraud.Worker.Domain.Transaction;

public class TransactionStatusEvent
{
    public Guid TransactionId { get; }
    public Status Status { get; }
    public TransactionStatusEvent(Guid transactionId, Status status)
    {
        TransactionId = transactionId;
        Status = status;
    }
}
