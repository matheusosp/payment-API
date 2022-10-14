namespace PaymentAPI.Domain.Features.Enums
{
    public enum SaleStatus
    {
        AwaitingPayment = 1,
        ApprovedPayment = 2,
        SentToCarrier = 3,
        Delivered = 4,
        Canceled = 5
    }
}
