using System.ComponentModel.DataAnnotations;
using TransactionService.Domain.Entities.Transaction.Commands;
using TransactionService.Domain.Entities.Transaction.Enums;

namespace TransactionService.Api.Controllers.UseCase.Transaction.Create
{
    public class CreateTransactionRequest
    {
        [Required]
        public Guid SourceAccountId { get; set; }

        [Required]
        public Guid TarjetAccountId { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "TransferTypeId must be: 1(Internal), 2(External) or 3(Payment).")]
        public int TransferTypeId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "The value must be greater than zero.")]
        public decimal Amount { get; set; }

        public CreateTransactionCommand ToCreateAccountCommand()
        {
            return new CreateTransactionCommand
            {
                SourceAccountId = Domain.Entities.Transaction.ValueObjects.AccountId.Create(SourceAccountId),
                TarjetAccountId = Domain.Entities.Transaction.ValueObjects.AccountId.Create(TarjetAccountId),
                TransferType = (TransferType)TransferTypeId,
                Amount = Domain.Entities.Transaction.ValueObjects.Amount.Create(Amount)
            };
        }
    }
}
