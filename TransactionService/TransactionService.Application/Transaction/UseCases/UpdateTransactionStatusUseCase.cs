using TransactionService.Application.Transaction.Exceptions;
using TransactionService.Application.Transaction.Interfaces;
using TransactionService.Domain.Entities.Transaction.Commands;
using TransactionService.Domain.Entities.Transaction.Repositories;

namespace TransactionService.Application.Transaction.UseCases;

public class UpdateTransactionStatusUseCase : IUpdateTransactionStatus
{
    private readonly ITransactionRepository _transactionRepository;
    public UpdateTransactionStatusUseCase(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    public async Task ExecuteAsync(UpdateTransactionCommand command, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var transaction = await _transactionRepository.GetByIdAsync(command.TransactionId, cancellationToken);

        if (transaction == null)
        {
            throw new TransactionDoesNotExistException(command.TransactionId);
        }

        transaction.UpdateStatus(command.Status);
        await _transactionRepository.UpdateAsync(transaction, cancellationToken);
    }
}
