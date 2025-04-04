using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TransactionService.Application.DependencyInjection;
using TransactionService.Infrastructure.DependencyInjection;
using TransactionService.Infrastructure.EntityFramework;
using TransactionService.Infrastructure.MessageBus.Kafka.Subscriber;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TransactionContext>();

builder.Services.AddUseCases();
builder.Services.AddRepositories(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddEventHandlersUseCases();

//Register Kafka Worker Service
builder.Services.AddHostedService<TransactionsValidatedSubscriber>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var sqliteContext = scope.ServiceProvider.GetRequiredService<TransactionContext>();
    sqliteContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
