using PaymentAPI.Domain.Features;
using PaymentAPI.Domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Commands
{
    public class UpdateSaleCommand
    {
        public Seller Seller { get; set; }
        public DateTime Date { get; set; }
        public SaleStatus Status { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
