using System.ComponentModel;

namespace TransactionService.Domain.Entities.Transaction.Enums;

public enum Status
{
    [Description("Pending")]
    Pending = 1,
    [Description("Approved")]
    Approved = 2,
    [Description("Rejected")]
    Rejected = 3
}