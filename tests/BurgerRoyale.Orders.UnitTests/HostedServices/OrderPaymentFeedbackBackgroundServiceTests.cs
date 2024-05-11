using BurgerRoyale.Orders.Domain.Configuration;
using BurgerRoyale.Orders.Domain.DTO;
using BurgerRoyale.Orders.Domain.Interface.IntegrationServices;
using BurgerRoyale.Orders.Domain.Interface.Services;
using BurgerRoyale.Orders.HostedServices.Services;
using BurgerRoyale.Orders.UnitTests.Domain.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace BurgerRoyale.Orders.UnitTests.HostedServices;

public class OrderPaymentFeedbackBackgroundServiceTests
{
    private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
    private readonly Mock<IOptions<MessageQueuesConfiguration>> _messageQueuesConfigurationMock;
    private readonly Mock<IServiceProvider> _serviceProviderMock;

    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly Mock<ILogger<OrderPaymentFeedbackBackgroundService>> _loggerMock;
    private readonly Mock<IMessageService> _messageServiceMock;

    private readonly OrderPaymentFeedbackBackgroundService _backgroundService;

    public OrderPaymentFeedbackBackgroundServiceTests()
    {
        _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        _messageQueuesConfigurationMock = new Mock<IOptions<MessageQueuesConfiguration>>();
        _serviceProviderMock = new Mock<IServiceProvider>();

        _orderServiceMock = new Mock<IOrderService>();
        _loggerMock = new Mock<ILogger<OrderPaymentFeedbackBackgroundService>>();
        _messageServiceMock = new Mock<IMessageService>();

        _messageQueuesConfigurationMock
            .Setup(x => x.Value)
            .Returns(MessageQueuesConfigurationMock.Get());

        _serviceProviderMock
            .Setup(x => x.GetService(typeof(IOrderService)))
            .Returns(_orderServiceMock.Object);

        _serviceProviderMock
            .Setup(x => x.GetService(typeof(ILogger<OrderPaymentFeedbackBackgroundService>)))
            .Returns(_loggerMock.Object);

        _serviceProviderMock
            .Setup(x => x.GetService(typeof(IMessageService)))
            .Returns(_messageServiceMock.Object);

        var serviceScope = new Mock<IServiceScope>();
            serviceScope
            .Setup(x => x.ServiceProvider)
            .Returns(_serviceProviderMock.Object);

        _serviceScopeFactoryMock
            .Setup(x => x.CreateScope())
            .Returns(serviceScope.Object);

        _backgroundService = new OrderPaymentFeedbackBackgroundService(
            _serviceScopeFactoryMock.Object,
            _messageQueuesConfigurationMock.Object
        );
    }

    [Fact]
    public async Task GivenBackgroundService_WhenOccursExceptionInSpecificMessageProcess_ThenShouldLogErrorOnlyForMessageWithErrors()
    {
        // arrange
        CancellationToken cancellationToken = new();

        _messageServiceMock
            .SetupSequence(x => x.ReadMessagesAsync<PaymentFeedbackDto>(
                It.IsAny<string>(),
                It.IsAny<int>()
            ))
            .ReturnsAsync(new List<PaymentFeedbackDto> {
                new(Guid.NewGuid(), true),
                new(Guid.NewGuid(), false)
            })
            .ReturnsAsync(
                new List<PaymentFeedbackDto>()
            );

        _orderServiceMock
            .SetupSequence(x => x.UpdatePaymentStatusAsync(
                It.IsAny<Guid>(),
                It.IsAny<bool>()
            ))
            .ThrowsAsync(new Exception())
            .Returns(() => Task.CompletedTask);

        // act
        await _backgroundService.StartAsync(cancellationToken);
        await _backgroundService.StopAsync(cancellationToken);

        // assert
        _orderServiceMock
            .Verify(
                x => x.UpdatePaymentStatusAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<bool>()
                ),
                Times.Exactly(2)
            );

        _loggerMock
            .Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Exactly(1)
            );
    }
}
