using TransactionService.Domain.Entities.Transaction.Enums;
using TransactionService.Domain.Entities.Transaction.ValueObjects;

namespace TransactionService.Domain.Entities.Transaction.Commands;

public class CreateTransactionCommand
{
    public AccountId SourceAccountId { get; set; }

    public AccountId TarjetAccountId { get; set; }

    public TransferType TransferType { get; set; }

    public Amount Amount { get; set; }
}
