using PaymentAPI.Domain.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Domain.Contracts
{
    public interface ISaleRepository
    {
        Sale Add(Sale sale);
    }
}
