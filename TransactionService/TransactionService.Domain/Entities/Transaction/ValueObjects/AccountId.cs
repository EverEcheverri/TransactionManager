using TransactionService.Domain.Entities.Transaction.Exceptions;

namespace TransactionService.Domain.Entities.Transaction.ValueObjects;

public record AccountId
{
    private AccountId(Guid value) => Value = value;

    public Guid Value { get; init; }

    public static AccountId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new NoValidAccountIdException();
        }

        return new AccountId(value);
    }
}
