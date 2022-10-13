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
            return Context.Sales.Include(s => s.Seller).Include(s => s.Items).FirstOrDefault(e => e.Id == id);
        }

        public Sale UpdateSaleById(Sale sale, Sale databaseSale)
        {
            //Atualizando Itens
            Context.Items.RemoveRange(databaseSale.Items);
            Context.Items.AddRange(sale.Items);

            //Atualizando Seller
            databaseSale.Seller.Name = sale.Seller.Name;
            databaseSale.Seller.Phone = sale.Seller.Phone;
            databaseSale.Seller.CPF = sale.Seller.CPF;
            databaseSale.Seller.Email = sale.Seller.Email;

            //Atualizando Sale
            databaseSale.Status = sale.Status;
            databaseSale.Date = sale.Date;
            Context.Entry(databaseSale).State = EntityState.Modified;

            Context.SaveChanges();
            return sale;
        }
    }
}
