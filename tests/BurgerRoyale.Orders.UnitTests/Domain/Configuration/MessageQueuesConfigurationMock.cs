using BurgerRoyale.Orders.Domain.Configuration;

namespace BurgerRoyale.Orders.UnitTests.Domain.Configuration;

public static class MessageQueuesConfigurationMock
{
    public static MessageQueuesConfiguration Get()
    {
        return new MessageQueuesConfiguration {
            OrderPaymentFeedbackQueue = "OrderPaymentFeedbackQueue",
            OrderPaymentRequestQueue = "OrderPaymentRequestQueue",
            OrderPreparationRequestQueue = "OrderPreparationRequestQueue",
            OrderPreparedQueue = "OrderPreparedQueue"
        };
    }
}
