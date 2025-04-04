using TransactionService.Domain.SharedKernel.Exceptions;

namespace TransactionService.Domain.Entities.Transaction.Exceptions;

public class NoValidSourceAccountIdException : BusinessException
{
    public NoValidSourceAccountIdException() : base("Source account ID cannot be empty.")
    {
    }
}
