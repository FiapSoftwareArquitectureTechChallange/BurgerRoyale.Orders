namespace BurgerRoyale.Orders.Domain.Configuration
{
    public class MessageQueuesConfiguration
    {
        public string OrderPaymentRequestQueue { get; set; } = string.Empty;
        public string OrderPaymentFeedbackQueue { get; set; } = string.Empty;
        public string OrderPreparationRequestQueue { get; set; } = string.Empty;
        public string OrderPreparedQueue { get; set; } = string.Empty;
    }
}