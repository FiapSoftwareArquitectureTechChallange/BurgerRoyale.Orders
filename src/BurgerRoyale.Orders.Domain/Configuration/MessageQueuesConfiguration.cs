namespace BurgerRoyale.Orders.Domain.Configuration
{
    public class MessageQueuesConfiguration
    {
        public string OrderPaymentRequestQueue { get; set; } = string.Empty;
        public string OrderPaymentResponseQueue { get; set; } = string.Empty;
        public string OrderPreparationRequestQueue { get; set; } = string.Empty;
    }
}
