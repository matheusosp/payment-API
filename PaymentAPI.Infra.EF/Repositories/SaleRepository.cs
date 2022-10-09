using Microsoft.EntityFrameworkCore;
using PaymentAPI.Domain.Features;
using PaymentAPI.Domain.Interfaces;
using PaymentAPI.Infra.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentAPI.Infra.EF.Repositories
{
    public class SaleRepository : GenericRepository, ISaleRepository
    {
        public SaleRepository(ApplicationDbContext context) : base(context)
        {
        }
        public Sale Add(Sale sale)
        {
            Context.Sales.Add(sale);
            Context.SaveChanges();

            return sale;
        }
        public Sale GetById(long id)
        {
            return Context.Sales.Include(s => s.Seller).Include(s => s.Items).FirstOrDefault(e => e.Id == id);
        }
    }
}
