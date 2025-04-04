using TransactionService.Domain.Entities.Transaction.Enums;
using TransactionService.Domain.Entities.Transaction.ValueObjects;

namespace TransactionService.Domain.Entities.Transaction.Commands;

public class UpdateTransactionCommand
{
    public Guid TransactionId { get; set; }
    public Status Status { get; set; }
}
