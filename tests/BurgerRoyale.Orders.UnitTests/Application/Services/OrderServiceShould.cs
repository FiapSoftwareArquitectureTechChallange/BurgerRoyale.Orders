using BurgerRoyale.Orders.Application.Services;
using BurgerRoyale.Orders.Domain.Configuration;
using BurgerRoyale.Orders.Domain.DTO;
using BurgerRoyale.Orders.Domain.Entities;
using BurgerRoyale.Orders.Domain.Enumerators;
using BurgerRoyale.Orders.Domain.Helpers;
using BurgerRoyale.Orders.Domain.Interface.IntegrationServices;
using BurgerRoyale.Orders.Domain.Interface.Repositories;
using BurgerRoyale.Orders.Domain.Interface.Services;
using BurgerRoyale.Orders.UnitTests.Domain.EntitiesMocks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace BurgerRoyale.Orders.UnitTests.Application.Services;

public class OrderServiceShould
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IMessageService> _messageServiceMock;
    private readonly Mock<IOptions<MessageQueuesConfiguration>> _messageQueuesConfigurationMock;
    private readonly Mock<ILogger<OrderService>> _loggerMock;

    private readonly IOrderService _orderService;

    public OrderServiceShould()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _messageServiceMock = new Mock<IMessageService>();
        _messageQueuesConfigurationMock = new Mock<IOptions<MessageQueuesConfiguration>>();
        _loggerMock = new Mock<ILogger<OrderService>>();

        _messageQueuesConfigurationMock
            .Setup(x => x.Value)
            .Returns(new MessageQueuesConfiguration
            {
                OrderPaymentFeedbackQueue = "OrderPaymentFeedbackQueue",
                OrderPaymentRequestQueue = "OrderPaymentRequestQueue",
                OrderPreparationRequestQueue = "OrderPreparationRequestQueue",
                OrderPreparedQueue = "OrderPreparedQueue"
            });

        _orderService = new OrderService(
            _orderRepositoryMock.Object, 
            _productRepositoryMock.Object,
            _httpContextAccessorMock.Object,
            _messageServiceMock.Object,
            _messageQueuesConfigurationMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Create_New_Order()
    {
        #region Arrange(Given)

        // Usuário
        var userId = Guid.NewGuid();

        //Produto
        var productId = Guid.NewGuid();
        var product = new Product("Burger", "Big burger", 20, ProductCategory.Lanche);

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(productId))
            .ReturnsAsync(product);

        _orderRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Order, bool>>>()))
            .ReturnsAsync(true);

        var order = OrderMock.Get();
        order.SetOrderNumber(1);

        _orderRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Order>
            {
                order
            });

        CreateOrderProductDTO orderProduct = new CreateOrderProductDTO()
        {
            ProductId = productId,
            Quantity = 1
        };

        var orderProducts = new List<CreateOrderProductDTO>
        {
            orderProduct
        };

        //Pedido
        CreateOrderDTO orderDTO = new()
        {
            OrderProducts = orderProducts
        };

        #endregion Arrange(Given)

        #region Act(When)

        var exception = await Record.ExceptionAsync(async () => await _orderService.CreateAsync(orderDTO));

        #endregion Act(When)

        #region Assert(Then)

        Assert.Null(exception);

        _orderRepositoryMock
            .Verify(
                repository => repository.AddAsync(It.Is<Order>(order =>
                    order.OrderProducts.Count() == 1)),
                Times.Once());

        _messageServiceMock
            .Verify(
                service => service.SendMessageAsync(
                    "OrderPaymentRequestQueue",
                    It.IsAny<PaymentRequestDto>()
                ),
                Times.Once
            );

        #endregion Assert(Then)
    }

    [Fact]
    public async Task CreateOrder_WithInvalidProduct_ThenShouldGiveAnException()
    {
        #region Arrange(Given)

        // Usuário
        var userId = Guid.NewGuid();

        //Produto
        var productId = Guid.NewGuid();
        var product = new Product("Burger", "Big burger", 20, ProductCategory.Lanche);

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(productId))
            .ReturnsAsync(product);

        CreateOrderProductDTO orderProduct = new CreateOrderProductDTO()
        {
            ProductId = Guid.NewGuid(),
            Quantity = 1
        };

        var orderProducts = new List<CreateOrderProductDTO>
        {
            orderProduct
        };

        //Pedido
        CreateOrderDTO orderDTO = new()
        {
            OrderProducts = orderProducts
        };

        #endregion Arrange(Given)

        #region Act(When)

        var exception = await Record.ExceptionAsync(async () => await _orderService.CreateAsync(orderDTO));

        #endregion Act(When)

        #region Assert(Then)

        Assert.NotNull(exception);
        Assert.Equal("Invalid(s) product(s).", exception.Message);

        #endregion Assert(Then)
    }

    [Fact]
    public async Task Get_Orders()
    {
        #region Arrange(Given)

        // Produto
        var productId = Guid.NewGuid();
        var productName = "Test";
        var productDesc = "Test description";
        var orderId = Guid.NewGuid();
        decimal productPrice = 30;
        int quantity = 1;
        var productCategory = ProductCategory.Sobremesa;

        var product = new Product(productName, productDesc, productPrice, productCategory);

        // Order product
        OrderProduct orderProduct = new OrderProduct(orderId, productId, productPrice, quantity, product);

        var userId = Guid.NewGuid();
        var orderStatus = OrderStatus.EmPreparacao;

        //Pedido
        Order order = new(userId);
        order.AddProduct(orderProduct);
        order.SetStatus(orderStatus);

        var orderList = new List<Order>
        {
            order
        };

        _orderRepositoryMock
            .Setup(x => x.GetOrders(null, null))
            .ReturnsAsync(orderList);

        #endregion Arrange(Given)

        #region Act(When)

        var orders = await _orderService.GetOrdersAsync(null);

        #endregion Act(When)

        #region Assert(Then)

        _orderRepositoryMock
            .Verify(
                repository => repository.GetOrders(null, null),
                Times.Once());

        Assert.NotNull(orders);
        Assert.Single(orders);
        Assert.Equal(30, orders.FirstOrDefault()?.TotalPrice);
        Assert.Equal(OrderStatus.EmPreparacao.GetDescription(), orders.FirstOrDefault()?.Status);

        #endregion Assert(Then)
    }

    [Fact]
    public async Task Get_Order()
    {
        #region Arrange(Given)

        // Produto
        var productId = Guid.NewGuid();
        var productName = "Test";
        var productDesc = "Test description";
        var orderId = Guid.NewGuid();
        decimal productPrice = 30;
        int quantity = 1;
        var productCategory = ProductCategory.Sobremesa;

        var product = new Product(productName, productDesc, productPrice, productCategory);

        // Order product
        OrderProduct orderProduct = new OrderProduct(orderId, productId, productPrice, quantity, product);

        var userId = Guid.NewGuid();
        var orderStatus = OrderStatus.EmPreparacao;

        //Pedido
        Order order = new(userId);
        order.AddProduct(orderProduct);
        order.SetStatus(orderStatus);

        _orderRepositoryMock
            .Setup(x => x.GetOrder(orderId, null))
            .ReturnsAsync(order);

        #endregion Arrange(Given)

        #region Act(When)

        var orderResponse = await _orderService.GetUserOrderAsync(orderId);

        #endregion Act(When)

        #region Assert(Then)

        _orderRepositoryMock
            .Verify(
                repository => repository.GetOrder(orderId, null),
                Times.Once());

        Assert.NotNull(orderResponse);

        #endregion Assert(Then)
    }

    [Fact]
    public async Task GetOrder_UnvalidOrder_ShouldThrowException()
    {
        #region Arrange(Given)

        // Produto
        var orderId = Guid.NewGuid();

        _orderRepositoryMock
            .Setup(x => x.GetOrder(orderId, null))
            .ReturnsAsync(null as Order);

        #endregion Arrange(Given)

        #region Act(When)

        var exception = await Record.ExceptionAsync(async () => await _orderService.GetUserOrderAsync(orderId));

        #endregion Act(When)

        #region Assert(Then)

        Assert.NotNull(exception);
        Assert.Equal("Order doesn't exist.", exception.Message);

        #endregion Assert(Then)
    }

    [Fact]
    public async Task Remove_Order()
    {
        #region Arrange(Given)

        // Produto
        var productId = Guid.NewGuid();
        var productName = "Test";
        var productDesc = "Test description";
        var orderId = Guid.NewGuid();
        decimal productPrice = 30;
        int quantity = 1;
        var productCategory = ProductCategory.Sobremesa;

        var product = new Product(productName, productDesc, productPrice, productCategory);

        // Order product
        OrderProduct orderProduct = new OrderProduct(orderId, productId, productPrice, quantity, product);

        var userId = Guid.NewGuid();
        var orderStatus = OrderStatus.EmPreparacao;

        //Pedido
        Order order = new(userId);
        order.AddProduct(orderProduct);
        order.SetStatus(orderStatus);

        var orderList = new List<Order>
        {
            order
        };

        _orderRepositoryMock
            .Setup(x => x.GetOrders(null, null))
            .ReturnsAsync(orderList);

        _orderRepositoryMock
            .Setup(x => x.GetOrder(orderId))
            .ReturnsAsync(order);

        #endregion Arrange(Given)

        #region Act(When)

        var exception = await Record.ExceptionAsync(async () => await _orderService.RemoveAsync(orderId));

        #endregion Act(When)

        #region Assert(Then)

        Assert.Null(exception);

        _orderRepositoryMock
            .Verify(
                repository => repository.Remove(It.Is<Order>(order =>
                    order.OrderProducts.Count() == 1 &&
                    order.UserId == order.UserId)),
                Times.Once());

        #endregion Assert(Then)
    }

    [Fact]
    public async Task RemoveOrder_UsingInvalidOrderId_ThenShouldGiveAnException()
    {
        #region Arrange(Given)

        // Produto
        var productId = Guid.NewGuid();
        var productName = "Test";
        var productDesc = "Test description";
        var orderId = Guid.NewGuid();
        decimal productPrice = 30;
        int quantity = 1;
        var productCategory = ProductCategory.Sobremesa;

        var product = new Product(productName, productDesc, productPrice, productCategory);

        // Order product
        OrderProduct orderProduct = new OrderProduct(orderId, productId, productPrice, quantity, product);

        var userId = Guid.NewGuid();
        var orderStatus = OrderStatus.EmPreparacao;

        //Pedido
        Order order = new(userId);
        order.AddProduct(orderProduct);
        order.SetStatus(orderStatus);

        var orderList = new List<Order>
        {
            order
        };

        _orderRepositoryMock
            .Setup(x => x.GetOrders(null, null))
            .ReturnsAsync(orderList);

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(order);

        #endregion Arrange(Given)

        #region Act(When)

        var exception = await Record.ExceptionAsync(async () => await _orderService.RemoveAsync(Guid.NewGuid()));

        #endregion Act(When)

        #region Assert(Then)

        Assert.NotNull(exception);
        Assert.Equal("Order doesn't exist.", exception.Message);

        _orderRepositoryMock
            .Verify(
                repository => repository.Remove(It.Is<Order>(order =>
                    order.OrderProducts.Count() == 1 &&
                    order.UserId == order.UserId)),
                Times.Never());

        #endregion Assert(Then)
    }

    [Fact]
    public async Task Update_Order_Status()
    {
        #region Arrange(Given)

        // Produto
        var productId = Guid.NewGuid();
        var productName = "Test";
        var productDesc = "Test description";
        var orderId = Guid.NewGuid();
        decimal productPrice = 30;
        int quantity = 1;
        var productCategory = ProductCategory.Sobremesa;

        var product = new Product(productName, productDesc, productPrice, productCategory);

        // Order product
        OrderProduct orderProduct = new OrderProduct(orderId, productId, productPrice, quantity, product);

        var userId = Guid.NewGuid();
        var orderStatus = OrderStatus.EmPreparacao;

        //Pedido
        Order order = new(userId);
        order.AddProduct(orderProduct);
        order.SetStatus(orderStatus);

        var orderList = new List<Order>
        {
            order
        };

        _orderRepositoryMock
            .Setup(x => x.GetOrders(null, null))
            .ReturnsAsync(orderList);

        _orderRepositoryMock
            .Setup(x => x.GetOrder(orderId))
            .ReturnsAsync(order);

        #endregion Arrange(Given)

        #region Act(When)

        var exception = await Record.ExceptionAsync(async () => await _orderService.UpdateOrderStatusAsync(orderId, OrderStatus.Pronto));

        #endregion Act(When)

        #region Assert(Then)

        Assert.Null(exception);

        _orderRepositoryMock
            .Verify(
                repository => repository.UpdateAsync(It.Is<Order>(order =>
                    order.OrderProducts.Count() == 1 &&
                    order.UserId == order.UserId)),
                Times.Once());

        #endregion Assert(Then)
    }

    [Fact]
    public async Task UpdateStatus_WithInvalidOrderId_ThenShouldGiveAnException()
    {
        #region Arrange(Given)

        // Produto
        var productId = Guid.NewGuid();
        var productName = "Test";
        var productDesc = "Test description";
        var orderId = Guid.NewGuid();
        decimal productPrice = 30;
        int quantity = 1;
        var productCategory = ProductCategory.Sobremesa;

        var product = new Product(productName, productDesc, productPrice, productCategory);

        // Order product
        OrderProduct orderProduct = new OrderProduct(orderId, productId, productPrice, quantity, product);

        var userId = Guid.NewGuid();
        var orderStatus = OrderStatus.EmPreparacao;

        //Pedido
        Order order = new(userId);
        order.AddProduct(orderProduct);
        order.SetStatus(orderStatus);

        var orderList = new List<Order>
        {
            order
        };

        _orderRepositoryMock
            .Setup(x => x.GetOrders(null, null))
            .ReturnsAsync(orderList);

        _orderRepositoryMock
            .Setup(x => x.GetOrder(orderId))
            .ReturnsAsync(order);

        #endregion Arrange(Given)

        #region Act(When)

        var exception = await Record.ExceptionAsync(async () => await _orderService.UpdateOrderStatusAsync(Guid.NewGuid(), OrderStatus.Pronto));

        #endregion Act(When)

        #region Assert(Then)

        Assert.NotNull(exception);
        Assert.Equal("Order doesn't exist.", exception.Message);

        _orderRepositoryMock
            .Verify(
                repository => repository.UpdateAsync(It.Is<Order>(order =>
                    order.OrderProducts.Count() == 1 &&
                    order.UserId == order.UserId)),
                Times.Never());

        #endregion Assert(Then)
    }

    [Fact]
    public async Task UpdateStatus_WithSameStatus_ThenShouldGiveAnException()
    {
        #region Arrange(Given)

        // Produto
        var productId = Guid.NewGuid();
        var productName = "Test";
        var productDesc = "Test description";
        var orderId = Guid.NewGuid();
        decimal productPrice = 30;
        int quantity = 1;
        var productCategory = ProductCategory.Sobremesa;

        var product = new Product(productName, productDesc, productPrice, productCategory);

        // Order product
        OrderProduct orderProduct = new OrderProduct(orderId, productId, productPrice, quantity, product);

        var userId = Guid.NewGuid();
        var orderStatus = OrderStatus.EmPreparacao;

        //Pedido
        Order order = new(userId);
        order.AddProduct(orderProduct);
        order.SetStatus(orderStatus);

        var orderList = new List<Order>
        {
            order
        };

        _orderRepositoryMock
            .Setup(x => x.GetOrders(null, null))
            .ReturnsAsync(orderList);

        _orderRepositoryMock
            .Setup(x => x.GetOrder(orderId))
            .ReturnsAsync(order);

        #endregion Arrange(Given)

        #region Act(When)

        var exception = await Record.ExceptionAsync(async () => await _orderService.UpdateOrderStatusAsync(orderId, OrderStatus.EmPreparacao));

        #endregion Act(When)

        #region Assert(Then)

        Assert.NotNull(exception);
        Assert.Equal($"Order already has {orderStatus.GetDescription()} status", exception.Message);

        _orderRepositoryMock
            .Verify(
                repository => repository.UpdateAsync(It.Is<Order>(order =>
                    order.OrderProducts.Count() == 1 &&
                    order.UserId == order.UserId)),
                Times.Never());

        #endregion Assert(Then)
    }

    [Fact]
    public async Task GiveUpdatePaymentStatusAsync_WhenOrderNotWaitingPayment_ThenShouldThrowDomainException()
    {
        #region Arrange(Given)

        Order order = new(Guid.NewGuid());
        order.SetStatus(OrderStatus.PagamentoReprovado);

        _orderRepositoryMock
            .Setup(x => x.GetOrder(order.Id))
            .ReturnsAsync(order);

        #endregion Arrange(Given)

        #region Act(When)

        var exception = await Record.ExceptionAsync(async () => await _orderService.UpdatePaymentStatusAsync(order.Id, true));

        #endregion Act(When)

        #region Assert(Then)

        Assert.NotNull(exception);
        Assert.Equal("Order doesn't have pending payment", exception.Message);

        #endregion Assert(Then)
    }

    [Fact]
    public async Task GiveUpdatePaymentStatusAsync_WhenPaymentNotProcessedWithSuccess_ThenShouldUpdateStatus()
    {
        #region Arrange(Given)

        Order order = new(Guid.NewGuid());
        order.SetStatus(OrderStatus.PagamentoPendente);

        _orderRepositoryMock
            .Setup(x => x.GetOrder(order.Id))
            .ReturnsAsync(order);

        #endregion Arrange(Given)

        #region Act(When)

        var exception = await Record.ExceptionAsync(async () => await _orderService.UpdatePaymentStatusAsync(order.Id, false));

        #endregion Act(When)

        #region Assert(Then)

        Assert.Null(exception);

        _orderRepositoryMock
            .Verify(
                x => x.UpdateAsync(It.Is<Order>(y => y.Status == OrderStatus.PagamentoReprovado)),
                Times.Once
            );

        _messageServiceMock
            .Verify(
                x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<object>()),
                Times.Never
            );

        #endregion Assert(Then)
    }

    [Fact]
    public async Task GiveUpdatePaymentStatusAsync_WhenPaymentProcessedWithSuccess_ThenShouldUpdateStatusAndRequestPreparation()
    {
        #region Arrange(Given)

        Order order = new(Guid.NewGuid());
        order.SetStatus(OrderStatus.PagamentoPendente);

        _orderRepositoryMock
            .Setup(x => x.GetOrder(order.Id))
            .ReturnsAsync(order);

        #endregion Arrange(Given)

        #region Act(When)

        var exception = await Record.ExceptionAsync(async () => await _orderService.UpdatePaymentStatusAsync(order.Id, true));

        #endregion Act(When)

        #region Assert(Then)

        Assert.Null(exception);

        _orderRepositoryMock
            .Verify(
                x => x.UpdateAsync(It.Is<Order>(y => y.Status == OrderStatus.EmPreparacao)),
                Times.Once
            );

        _messageServiceMock
            .Verify(
                x => x.SendMessageAsync(It.IsAny<string>(), It.IsAny<object>()),
                Times.Once
            );

        #endregion Assert(Then)
    }
}