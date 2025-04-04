using System.Reflection;
using TransactionService.Domain.Entities.Transaction.Commands;
using TransactionService.Domain.Entities.Transaction.Enums;
using TransactionService.Domain.Entities.Transaction.ValueObjects;

namespace TransactionService.Test.Data.Commands;

public class CreateTransactionCommandMother
{
    public static CreateTransactionCommand Create(
    string sourceAccountId = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    string tarjetAccountId = "9989deb5-6a84-46f5-ae79-962528156f51",
    int transferType = 1,
    decimal amount = 1)
    {
        return new CreateTransactionCommand
        {
            SourceAccountId = AccountId.Create(Guid.Parse(sourceAccountId)),
            TarjetAccountId = AccountId.Create(Guid.Parse(tarjetAccountId)),
            TransferType = (TransferType)transferType,
            Amount = Amount.Create(amount)
        };
    }
}
