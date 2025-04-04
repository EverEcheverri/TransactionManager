using TransactionService.Domain.Entities.Transaction.Exceptions;

namespace TransactionService.Domain.Entities.Transaction.ValueObjects;

public record Amount
{
    private Amount(decimal value) => Value = value;

    public decimal Value { get; init; }

    public static Amount Create(decimal value)
    {
        if (value <= 0)
        {
            throw new InvalidAmountException();
        }

        return new Amount(value);
    }
}