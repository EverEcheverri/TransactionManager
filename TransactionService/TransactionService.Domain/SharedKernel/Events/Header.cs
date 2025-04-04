namespace TransactionService.Domain.SharedKernel.Events;

public class Header
{
    public readonly string ServiceId = "Transaction";
    public Header()
    {
        MessageId = Guid.NewGuid().ToString();
        EventGeneratedDateUtc = DateTime.UtcNow;
    }
    public Guid Key { get; set; }
    public string MessageId { get; }
    public string EventType { get; set; }
    public string Subject { get; set; }
    public DateTime EventGeneratedDateUtc { get; }
}
