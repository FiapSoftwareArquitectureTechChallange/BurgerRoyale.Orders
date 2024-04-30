using BurgerRoyale.Orders.Domain.Configuration;
using BurgerRoyale.Orders.Domain.DTO;
using BurgerRoyale.Orders.Domain.Interface.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BurgerRoyale.Orders.Infrastructure.HostedServices.SQSBackgroundServices;

public class OrderPaymentResponseBackgroundService : SQSBackgroundService<RequestPaymentResponseDto>
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderPaymentResponseBackgroundService> _logger;

    public OrderPaymentResponseBackgroundService
    (
        IServiceScopeFactory serviceScopeFactory,
        IOptions<MessageQueuesConfiguration> queuesConfiguration
    ) : base(serviceScopeFactory, queuesConfiguration.Value.OrderPaymentResponseQueue)
    {
        _orderService = _serviceProvider.GetRequiredService<IOrderService>();
        _logger = _serviceProvider.GetRequiredService<ILogger<OrderPaymentResponseBackgroundService>>();
    }

    protected override async Task ProcessMessage(RequestPaymentResponseDto message)
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
                "Error processing order {OrderId} payment response",
                message.OrderId
            );
        }
        
    }
}
