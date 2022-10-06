using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using payment_api.Context;
using payment_api.Entities;
using payment_api.Entities.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace payment_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        public readonly SaleContext _context;
        public SaleController(SaleContext context)
        {
            _context = context;
        }

        // GET api/<SaleController>/5
        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var sale = _context.Sales.Find(id);

            if (sale == null)
                return NotFound();

            return Ok(sale);
        }

        // POST api/<SaleController>
        [HttpPost]
        public IActionResult Create(Sale sale)
        {
            sale.Status = SaleStatus.AwaitingPayment;
            _context.Add(sale);
            _context.SaveChanges();
            return Ok(sale);
        }

        // PUT api/<SaleController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateById(int id, Sale sale)
        {
            var saleDatabase = _context.Sales.Find(id);

            if (saleDatabase == null)
                return NotFound();

            saleDatabase.Status = sale.Status;
            saleDatabase.Seller = sale.Seller;
            saleDatabase.Items = sale.Items;
            saleDatabase.Date = sale.Date;

            _context.Update(saleDatabase);
            _context.SaveChanges();

            return Ok(saleDatabase);
        }

    }
}
