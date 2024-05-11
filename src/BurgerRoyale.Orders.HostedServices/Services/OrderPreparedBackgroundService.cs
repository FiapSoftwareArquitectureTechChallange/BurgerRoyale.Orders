using BurgerRoyale.Orders.Domain.Configuration;
using BurgerRoyale.Orders.Domain.DTO;
using BurgerRoyale.Orders.Domain.Enumerators;
using BurgerRoyale.Orders.Domain.Interface.Services;
using BurgerRoyale.Orders.HostedServices.Services.Common;
using Microsoft.Extensions.Options;

namespace BurgerRoyale.Orders.HostedServices.Services;

public class OrderPreparedBackgroundService : SqsBackgroundService<OrderPreparedDto>
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderPreparedBackgroundService> _logger;

    public OrderPreparedBackgroundService
    (
        IServiceScopeFactory serviceScopeFactory,
        IOptions<MessageQueuesConfiguration> queuesConfiguration
    ) : base(serviceScopeFactory, queuesConfiguration.Value.OrderPreparedQueue)
    {
        _orderService = _serviceProvider.GetRequiredService<IOrderService>();
        _logger = _serviceProvider.GetRequiredService<ILogger<OrderPreparedBackgroundService>>();
    }

    protected override async Task ProcessMessage(OrderPreparedDto message)
    {
        try
        {
            await _orderService.UpdateOrderStatusAsync(
                message.OrderId,
                OrderStatus.Pronto
            );
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Error processing order {OrderId} prepared message",
                message.OrderId
            );
        }

    }
}
