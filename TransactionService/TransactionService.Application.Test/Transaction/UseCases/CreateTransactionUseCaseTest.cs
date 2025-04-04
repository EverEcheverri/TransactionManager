using NSubstitute;
using TransactionService.Application.Transaction.UseCases;
using TransactionService.Domain.Entities.Transaction.Enums;
using TransactionService.Domain.Entities.Transaction.Repositories;
using TransactionService.Test.Data.Commands;

namespace TransactionService.Application.Test.Transaction.UseCases;

public class CreateTransactionUseCaseTest
{
    private readonly CreateTransactionUseCase _useCase;
    private readonly ITransactionRepository _transactionRepositoryMock;

    public CreateTransactionUseCaseTest()
    {
        _transactionRepositoryMock = Substitute.For<ITransactionRepository>();
        _useCase = new CreateTransactionUseCase(_transactionRepositoryMock);
    }

    [Fact]
    public async Task CreateTransactionUseCase_ExecuteAsync_Successfully()
    {
        // Arrange
        var command = CreateTransactionCommandMother.Create();

        Domain.Entities.Transaction.Transaction? transaction = null;
        _transactionRepositoryMock.When(a => a.SaveAsync(Arg.Any<Domain.Entities.Transaction.Transaction>(), CancellationToken.None))
            .Do(call => transaction = call.ArgAt<Domain.Entities.Transaction.Transaction>(0));

        // Act
        var transactionId = await _useCase.ExecuteAsync(command, CancellationToken.None);

        // Asserts Mocks
        await _transactionRepositoryMock.Received().SaveAsync(transaction!, CancellationToken.None);

        // Asserts callback
        Assert.NotNull(transaction);
        Assert.Equal(transactionId, transaction.Id);
        Assert.Equal(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), transaction.SourceAccountId.Value);
        Assert.Equal(Guid.Parse("9989deb5-6a84-46f5-ae79-962528156f51"), transaction.TarjetAccountId.Value);
        Assert.Equal((TransferType)1, transaction.TransferType);
        Assert.Equal(1, transaction.Amount.Value);
    }
}
