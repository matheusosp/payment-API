namespace PaymentAPI.Domain.Features.Enums
{
    public enum SaleStatus
    {
        AwaitingPayment,
        ApprovedPayment,
        SentToCarrier,
        Delivered,
        Canceled
    }
}
