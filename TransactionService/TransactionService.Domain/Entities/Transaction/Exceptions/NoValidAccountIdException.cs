using TransactionService.Domain.SharedKernel.Exceptions;

namespace TransactionService.Domain.Entities.Transaction.Exceptions;

public class NoValidAccountIdException : BusinessException
{
    public NoValidAccountIdException() : base("Tarjet account ID cannot be empty.")
    {
    }
}
