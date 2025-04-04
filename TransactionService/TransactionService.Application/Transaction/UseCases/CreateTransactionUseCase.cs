using TransactionService.Application.Transaction.Interfaces;
using TransactionService.Domain.Entities.Transaction.Commands;
using TransactionService.Domain.Entities.Transaction.Repositories;

namespace TransactionService.Application.Transaction.UseCases;

public class CreateTransactionUseCase : ICreateTransaction
{
    private readonly ITransactionRepository _transactionRepository;
    public CreateTransactionUseCase(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Guid> ExecuteAsync(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var transaction = Domain.Entities.Transaction.Transaction.Build(
            command.SourceAccountId, command.TarjetAccountId, command.TransferType, command.Amount);

        await _transactionRepository.SaveAsync(transaction, cancellationToken);

        return transaction.Id;
    }
}
