using AntiFraud.Worker.Domain.SharedKernel;
using AntiFraud.Worker.Domain.Transaction;
using AntiFraud.Worker.Persistence.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AntiFraud.Worker.Persistence.EntityFramework;

public class AntiFraudContext : DbContext
{
    protected readonly IConfiguration _configuration;

    public AntiFraudContext()
    {

    }

    public AntiFraudContext(IConfiguration configuration, DbContextOptions<AntiFraudContext> options)
         : base(options)
    {
        _configuration = configuration;
    }


    public DbSet<Transaction> Transactions { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AuditShadowProperties();
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    private void AuditShadowProperties()
    {
        var timestamp = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries()
          .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            var updatedProperty = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "Updated");
            var createdProperty = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "Created");

            if (updatedProperty == null || createdProperty == null)
            {
                continue;
            }

            updatedProperty.CurrentValue = timestamp;

            if (entry.State == EntityState.Added && createdProperty.Metadata.IsShadowProperty())
            {
                createdProperty.CurrentValue = timestamp;
            }
        }
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = GetConnectionStringPath();
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite($"Data Source={connectionString};")
            .LogTo(Console.WriteLine, LogLevel.Information);
        }
        optionsBuilder.EnableSensitiveDataLogging();
    }

    private string GetConnectionStringPath()
    {
        var relativePath = _configuration.GetConnectionString("DefaultConnection");
        var absolutePath = Path.GetFullPath(relativePath!);
        return absolutePath;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.ApplyConfiguration(new TransactionEntityTypeConfiguration());

        base.OnModelCreating(builder);
    }
}
