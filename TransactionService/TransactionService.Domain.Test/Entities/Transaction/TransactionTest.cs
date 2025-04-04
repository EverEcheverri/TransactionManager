using TransactionService.Domain.Entities.Transaction.Enums;
using TransactionService.Test.Data.Transaction;

namespace TransactionService.Domain.Test.Entities.Transaction;

public class TransactionTest
{
    [Fact]
    public void Transaction_Builds_Successfully()
    {
        // Act
        var transaction = TransactionMother.Create();

        // Assert
        Assert.NotNull(transaction);
        Assert.Equal(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), transaction.SourceAccountId.Value);
        Assert.Equal(Guid.Parse("9989deb5-6a84-46f5-ae79-962528156f51"), transaction.TarjetAccountId.Value);
        Assert.Equal((TransferType)1, transaction.TransferType);
        Assert.Equal(1, transaction.Amount.Value);
    }
}