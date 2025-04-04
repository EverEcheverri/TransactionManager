using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TransactionService.Domain.SharedKernel.Events;
using TransactionService.Domain.SharedKernel;
using System.Reflection;
using TransactionService.Infrastructure.EntityFramework.Confgurations;

namespace TransactionService.Infrastructure.EntityFramework;

public class TransactionContext : DbContext
{
    protected readonly IConfiguration _configuration;

    private readonly MediatR.IPublisher _publisher;
    private readonly ILogger<TransactionContext> _logger;
    public TransactionContext()
    {

    }

    public TransactionContext(
        ILogger<TransactionContext> logger,
        IConfiguration configuration,
        DbContextOptions<TransactionContext> options,
        MediatR.IPublisher publisher) : base(options)
    {
        _publisher = publisher;
        _logger = logger;
        _configuration = configuration;
    }

    public DbSet<Domain.Entities.Transaction.Transaction> Transactions { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AuditShadowProperties();
        var result = await base.SaveChangesAsync(cancellationToken);

        var events = ChangeTracker.Entries<Entity>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .ToArray();

        foreach (var @event in events)
        {
            var eventTypeClass = @event.GetType();
            if (eventTypeClass.IsGenericType && eventTypeClass.GetGenericTypeDefinition() == typeof(Event<>))
            {
                var eventData = eventTypeClass?.GetProperty("Body")?.GetValue(@event);
                _logger.LogInformation($"Event type: {eventData.GetType()}");

                await _publisher.Publish(eventData, cancellationToken);
            }
        }

        return result;
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
}
