using PaymentAPI.Domain.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Domain.Interfaces
{
    public interface ISaleRepository
    {
        Sale Add(Sale sale);
        Sale GetById(long id);
        Sale UpdateSaleById(Sale sale);
    }
}
