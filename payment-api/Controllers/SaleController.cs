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
        // GET api/<SaleController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> RetrieveSaleById(long id, CancellationToken cancellationToken)
        {
            var query = new RetrieveSaleByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsFailure)
                return NotFound();

            return Ok(result.Value);
        }

        // POST api/<SaleController>
        [HttpPost]
        public async Task<IActionResult> RegisterSale(RegisterSaleCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsFailure)
                return BadRequest(result.Error);

            return StatusCode(201, result.Value);
        }

        // PUT api/<SaleController>/5
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
