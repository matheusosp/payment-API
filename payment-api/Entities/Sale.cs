using payment_api.Entities.Enums;

namespace payment_api.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public Seller Seller { get; set; }
        public DateTime Date { get; set; }
        public SaleStatus Status { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
