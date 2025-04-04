using System.ComponentModel;

namespace AntiFraud.Worker.Domain.Transaction.Enums;

public enum Status
{
    [Description("Pending")]
    Pending = 1,
    [Description("Approved")]
    Approved = 2,
    [Description("Rejected")]
    Rejected = 3
}