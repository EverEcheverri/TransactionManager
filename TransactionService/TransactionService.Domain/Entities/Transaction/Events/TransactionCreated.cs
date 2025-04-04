using TransactionService.Domain.Entities.Transaction.Enums;
using TransactionService.Domain.Entities.Transaction.ValueObjects;
using TransactionService.Domain.SharedKernel.Events;

namespace TransactionService.Domain.Entities.Transaction.Events;

public class TransactionCreated : DomainEvent
{
    public Guid TransactionId { get; set; }
    public Guid SourceAccountId { get; set; }
    public Guid TarjetAccountId { get; set; }
    public int TransferType { get; set; }
    public decimal Amount { get; set; }
    public int Status { get; set; }
}
