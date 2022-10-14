using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentAPI.Application.Commands;
using PaymentAPI.Application.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace payment_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public SaleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Operação para pegar uma única venda da base de dados.
        /// </summary>
        /// <param name="id">Código identificador da venda</param>
        /// <returns>Um objeto de venda</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> RetrieveSaleById(long id, CancellationToken cancellationToken)
        {
            var query = new RetrieveSaleByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsFailure)
                return NotFound();

            return Ok(result.Value);
        }

        /// <summary>
        /// Operação que realiza o cadastro da venda
        /// </summary>
        /// <param name="palavra">Um objeto de venda</param>
        /// <returns>Um objeto de venda com seu Id</returns>
        [HttpPost]
        public async Task<IActionResult> RegisterSale(RegisterSaleCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsFailure)
                return BadRequest(result.Error);

            return StatusCode(201, result.Value);
        }

        /// <summary>
        /// Operação que realiza a substituição de dados de uma venda especifica.
        /// </summary>
        /// <param name="id">Código identificador da venda a ser alterada</param>
        /// <param name="palavra">Objeto venda com dados para alteração</param>
        /// <returns>Um objeto de venda com seu Id</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(long id, [FromBody] UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsFailure)
                return BadRequest(result.Error);

            return StatusCode(200, result.Value);
        }

    }
}
