using BurgerRoyale.Orders.Domain.Configuration;
using BurgerRoyale.Orders.Domain.DTO;
using BurgerRoyale.Orders.Domain.Interface.Services;
using BurgerRoyale.Orders.HostedServices.Services.Common;
using Microsoft.Extensions.Options;

namespace BurgerRoyale.Orders.HostedServices.Services;

public class OrderPaymentFeedbackBackgroundService : SqsBackgroundService<PaymentFeedbackDto>
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderPaymentFeedbackBackgroundService> _logger;

    public OrderPaymentFeedbackBackgroundService
    (
        IServiceScopeFactory serviceScopeFactory,
        IOptions<MessageQueuesConfiguration> queuesConfiguration
    ) : base(serviceScopeFactory, queuesConfiguration.Value.OrderPaymentFeedbackQueue)
    {
        _orderService = _serviceProvider.GetRequiredService<IOrderService>();
        _logger = _serviceProvider.GetRequiredService<ILogger<OrderPaymentFeedbackBackgroundService>>();
    }

    protected override async Task ProcessMessage(PaymentFeedbackDto message)
    {
        try
        {
            await _orderService.UpdatePaymentStatusAsync(
                message.OrderId,
                message.ProcessedSuccessfully
            );
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Error processing order {OrderId} payment feedback",
                message.OrderId
            );
        }

    }
}
