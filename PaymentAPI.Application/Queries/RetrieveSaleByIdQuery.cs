using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Queries
{
    public class RetrieveSaleByIdQuery
    {
        public int Id { get; set; }

        public RetrieveSaleByIdQuery(int id)
        {
            Id = id;
        }
    }
}
