using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TransactionService.Application.Transaction.Interfaces;

namespace TransactionService.Api.Controllers.UseCase.Transaction.Create
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ICreateTransaction _createTransaction;

        public TransactionController(ICreateTransaction createTransaction)
        {
            _createTransaction = createTransaction;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(typeof(CustomErrorResponse), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(CustomErrorResponse), StatusCodes.Status409Conflict)]
        //[ProducesResponseType(typeof(CustomErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTransaction([FromBody][Required] CreateTransactionRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.ToCreateAccountCommand();
            var transactionId = await _createTransaction.ExecuteAsync(command, cancellationToken);

            return StatusCode((int)HttpStatusCode.Created, transactionId);
        }
    }
}
