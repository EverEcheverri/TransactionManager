using AntiFraud.Worker.Domain.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AntiFraud.Worker.Persistence.EntityFramework.Configurations;

internal class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {

        builder.HasKey(c => c.TransactionId);
        builder.Property(c => c.TransactionId)
            .HasConversion<Guid>();

        builder.Property(p => p.AccountId)
        .HasColumnName(nameof(Transaction.AccountId))
        .HasConversion<Guid>();

        builder.Property(p => p.Amount)
        .HasColumnName(nameof(Transaction.Amount))
        .HasConversion<decimal>();

        builder.Property(p => p.Status)
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.Property(p => p.DateOccurredUtc)
       .HasColumnName(nameof(Transaction.DateOccurredUtc))
       .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
       .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(p => p.RejectionReason)
               .HasConversion<string>()
               .HasMaxLength(100);

        // Audit shadow properties
        builder.Property<DateTime>("Created");
        builder.Property<DateTime?>("Updated");
    }
}
