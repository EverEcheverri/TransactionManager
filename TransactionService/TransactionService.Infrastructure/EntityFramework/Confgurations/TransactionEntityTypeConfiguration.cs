using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities.Transaction.ValueObjects;

namespace TransactionService.Infrastructure.EntityFramework.Confgurations;

internal class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Entities.Transaction.Transaction>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Transaction.Transaction> builder)
    {
        builder.Ignore(p => p.DomainEvents);

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion<Guid>();

        builder.Property(p => p.SourceAccountId)
        .HasColumnName(nameof(Domain.Entities.Transaction.Transaction.SourceAccountId))
        .HasConversion<Guid>(p => p.Value, p => AccountId.Create(p));

        builder.Property(p => p.TarjetAccountId)
        .HasColumnName(nameof(Domain.Entities.Transaction.Transaction.TarjetAccountId))
        .HasConversion<Guid>(p => p.Value, p => AccountId.Create(p));

        builder.Property(c => c.TransferType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(p => p.Amount)
        .HasColumnName(nameof(Domain.Entities.Transaction.Transaction.Amount))
        .HasConversion<decimal>(p => p.Value, p => Amount.Create(p));

        builder.Property(p => p.Status)
               .HasConversion<string>()
               .HasMaxLength(20);

        // Audit shadow properties
        builder.Property<DateTime>("Created");
        builder.Property<DateTime?>("Updated");
    }
}
