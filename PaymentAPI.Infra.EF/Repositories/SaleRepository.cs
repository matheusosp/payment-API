using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using PaymentAPI.Domain.Features;
using PaymentAPI.Domain.Interfaces;
using PaymentAPI.Infra.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            return Context.Sales.AsNoTracking().Include(s => s.Seller).Include(s => s.Items).FirstOrDefault(e => e.Id == id);
        }

        public Sale UpdateSaleById(Sale sale, Sale databaseSale)
        {
            //Atualizar Itens
            Context.Items.RemoveRange(databaseSale.Items);

            Context.Sales.Update(sale);

            Context.SaveChanges();
            return sale;
        }
    }
}
