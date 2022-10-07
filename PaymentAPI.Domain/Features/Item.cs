using System.ComponentModel.DataAnnotations;

namespace PaymentAPI.Domain.Features
{
    public class Item
    {
        public int ItemId { get; set; }
        public int SaleId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
