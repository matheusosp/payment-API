using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentAPI.Application.Commands;
using System.ComponentModel.DataAnnotations;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Threading;
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
        //[HttpPut("{id}")]
        //public IActionResult UpdateById(int id, Sale sale)
        //{
        //    var saleDatabase = _context.Sales.Find(id);

        //    if (saleDatabase == null)
        //        return NotFound();

        //    saleDatabase.Status = sale.Status;
        //    saleDatabase.Seller = sale.Seller;
        //    saleDatabase.Items = sale.Items;
        //    saleDatabase.Date = sale.Date;

        //    _context.Update(saleDatabase);
        //    _context.SaveChanges();

        //    return Ok(saleDatabase);
        //}

    }
}
