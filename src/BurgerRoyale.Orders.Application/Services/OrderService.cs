using BurgerRoyale.Orders.Domain.Configuration;
using BurgerRoyale.Orders.Domain.Constants;
using BurgerRoyale.Orders.Domain.DTO;
using BurgerRoyale.Orders.Domain.Entities;
using BurgerRoyale.Orders.Domain.Enumerators;
using BurgerRoyale.Orders.Domain.Exceptions;
using BurgerRoyale.Orders.Domain.Helpers;
using BurgerRoyale.Orders.Domain.Interface.IntegrationServices;
using BurgerRoyale.Orders.Domain.Interface.Repositories;
using BurgerRoyale.Orders.Domain.Interface.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BurgerRoyale.Orders.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMessageService _messageService;
    private readonly MessageQueuesConfiguration _messageQueuesConfiguration;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IHttpContextAccessor httpContextAccessor,
        IMessageService messageService,
        IOptions<MessageQueuesConfiguration> messageQueuesConfiguration,
        ILogger<OrderService> logger
    )
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
        _messageService = messageService;
        _messageQueuesConfiguration = messageQueuesConfiguration.Value;
        _logger = logger;
    }

    public async Task<OrderDTO> CreateAsync(CreateOrderDTO orderDTO)
    {
        Order order = await CreateOrder(orderDTO);        

        await RequestOrderPayment(order);

        return new OrderDTO(order);
    }    

    private async Task<Order> CreateOrder(CreateOrderDTO orderDTO)
    {
        var userId = GetLoggedUserId();

        var order = new Order(userId);

        await AddOrderProductsToOrder(orderDTO, order);

        order.SetOrderNumber(await GenerateOrderNumber());

        await _orderRepository.AddAsync(order);

        _logger.LogInformation(
            "Order {OrderId} created",
            order.Id
        );

        return order;
    }

    private async Task AddOrderProductsToOrder(CreateOrderDTO orderDTO, Order order)
    {
        foreach (var orderProduct in orderDTO.OrderProducts)
        {
            var product = await _productRepository.GetByIdAsync(orderProduct.ProductId);
            ValidateIfProductDoesNotExist(product);

            var newOrderProduct = new OrderProduct(order.Id, orderProduct.ProductId, product!.Price, orderProduct.Quantity);

            order.AddProduct(newOrderProduct);
        }
    }

    private static void ValidateIfProductDoesNotExist(Product? product)
    {
        if (product is null)
            throw new NotFoundException("Invalid(s) product(s).");
    }

    public async Task<int> GenerateOrderNumber()
    {
        var anyUnclosedOrders = await _orderRepository.AnyAsync(x => x.Status == OrderStatus.Finalizado);
        if (anyUnclosedOrders)
        {
            var lastOrder = (await _orderRepository.GetAllAsync()).OrderByDescending(x => x.OrderTime).FirstOrDefault();
            return lastOrder.OrderNumber + 1;
        }
        return 1;
    }

    private async Task RequestOrderPayment(Order order)
    {
        var message = new RequestPaymentDto(order.Id, order.TotalPrice, order.UserId);

        await _messageService.SendMessageAsync(
            _messageQueuesConfiguration.OrderPaymentRequestQueue,
            message
        );

        _logger.LogInformation(
            "Order {OrderId} payment request sent",
            order.Id
        );
    }

    public async Task<IEnumerable<OrderDTO>> GetOrdersAsync(OrderStatus? orderStatus)
    {
        var userId = IsCustomer() ? GetLoggedUserId() : null;

        var orders = await _orderRepository.GetOrders(orderStatus, userId);

        var orderDTOs = orders.Select(order => new OrderDTO(order)).ToList();

        return orderDTOs;
    }

    public async Task<OrderDTO> GetUserOrderAsync(Guid id)
    {
        var userId = IsCustomer() ? GetLoggedUserId() : null;

        var order = await _orderRepository.GetOrder(id, userId);

        if (order is null)
            throw new NotFoundException("Order doesn't exist.");

        return new OrderDTO(order!);
    }

    public async Task RemoveAsync(Guid id)
    {
        Order order = await GetOrderById(id);

        _orderRepository.Remove(order!);

        _logger.LogInformation(
            "Order {OrderId} removed",
            order.Id
        );
    }

    public async Task UpdateOrderStatusAsync(Guid id, OrderStatus orderStatus)
    {
        var order = await GetOrderById(id);

        ValidateIfStatusIsTheSame(orderStatus, order);

        order!.SetStatus(orderStatus);

        await _orderRepository.UpdateAsync(order);
    }

    private async Task<Order> GetOrderById(Guid id)
    {
        var order = await _orderRepository.GetOrder(id);

        if (order is null)
            throw new NotFoundException("Order doesn't exist.");

        return order;
    }

    private static void ValidateIfStatusIsTheSame(OrderStatus orderStatus, Order order)
    {
        if (order.Status == orderStatus)
            throw new DomainException($"Order already has {orderStatus.GetDescription()} status");
    }

    private Guid? GetLoggedUserId()
    {
        var claim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(
            x => x.Type == ClaimTypes.NameIdentifier
        );

        if (Guid.TryParse(claim?.Value, out Guid userId))
        {
            return userId;
        }

        return null;
    }

    private bool IsCustomer()
    {
        return _httpContextAccessor.HttpContext?.User?.IsInRole(RolesConstants.Customer) ?? false;
    }

    public async Task UpdatePaymentStatusAsync(Guid id, bool paymentSuccesfullyProcessed)
    {
        var order = await GetOrderById(id);

        if (order.Status != OrderStatus.PagamentoPendente)
            throw new DomainException($"Order doesn't have pending payment");

        var newStatus = paymentSuccesfullyProcessed
                ? OrderStatus.EmPreparacao
                : OrderStatus.PagamentoReprovado;

        await UpdateOrderStatusAsync(
            id,
            newStatus
        );

        _logger.LogInformation(
            "Order {OrderId} updated to \"{OrderStatus}\" status",
            id,
            newStatus.GetDescription()
        );

        if (paymentSuccesfullyProcessed)
            await RequestOrderPreparation(new OrderDTO(order));
    }

    private async Task RequestOrderPreparation(OrderDTO orderDto)
    {
        var message = new RequestOrderPreparationDto(
            orderDto.OrderId,
            orderDto.OrderProducts,
            orderDto.UserId
        );

        await _messageService.SendMessageAsync(
            _messageQueuesConfiguration.OrderPreparationRequestQueue,
            message
        );

        _logger.LogInformation(
            "Order {OrderId} preparation requested",
            orderDto.OrderId
        );
    }
}
