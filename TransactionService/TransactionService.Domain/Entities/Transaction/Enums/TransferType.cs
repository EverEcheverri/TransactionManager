using System.ComponentModel;

namespace TransactionService.Domain.Entities.Transaction.Enums;

public enum TransferType
{
    [Description("Transfer between accounts in the same bank")]
    Internal = 1,
    [Description("Transfer to another bank")]
    External = 2,
    [Description("Payment to a merchant or service provider")]
    Payment = 3,
}