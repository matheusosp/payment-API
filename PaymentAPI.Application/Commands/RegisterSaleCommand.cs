using PaymentAPI.Domain.Features;
using PaymentAPI.Domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Commands
{
    public class RegisterSaleCommand
    {
        public Seller Seller { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
