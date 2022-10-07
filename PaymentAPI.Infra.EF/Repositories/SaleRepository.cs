using Microsoft.EntityFrameworkCore;
using PaymentAPI.Domain.Contracts;
using PaymentAPI.Domain.Features;
using PaymentAPI.Infra.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
