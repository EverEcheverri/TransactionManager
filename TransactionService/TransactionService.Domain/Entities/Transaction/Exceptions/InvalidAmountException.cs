using TransactionService.Domain.SharedKernel.Exceptions;

namespace TransactionService.Domain.Entities.Transaction.Exceptions;

public class InvalidAmountException : BusinessException
{
    public InvalidAmountException() : base("Transaction amount must be greater than zero.")
    {
    }
}
