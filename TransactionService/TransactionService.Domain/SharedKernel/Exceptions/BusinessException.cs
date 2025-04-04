namespace TransactionService.Domain.SharedKernel.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string message)
      : base(message)
    {
    }
}