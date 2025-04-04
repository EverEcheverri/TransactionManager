using TransactionService.Domain.SharedKernel.Exceptions;

namespace TransactionService.Application.Transaction.Exceptions;

internal class TransactionDoesNotExistException : BusinessException
{
    public TransactionDoesNotExistException(Guid transactionId) :
     base($"Transaction {transactionId} does not exist")
    {
    }
}
