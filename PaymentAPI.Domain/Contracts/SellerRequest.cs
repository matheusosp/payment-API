using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Domain.Contracts
{
    public class SellerRequest
    {
        public string Name { get; set; }
        public int CPF { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
    }
}
