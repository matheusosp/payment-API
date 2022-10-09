using PaymentAPI.Domain.Features.Enums;

namespace PaymentAPI.Domain.Features
{
    public class Sale
    {
        public long Id { get; set; }
        public Seller Seller { get; set; }
        public DateTime Date { get; set; }
        public SaleStatus Status { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
