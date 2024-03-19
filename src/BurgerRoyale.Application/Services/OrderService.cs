using BurgerRoyale.Domain.Constants;
using BurgerRoyale.Domain.DTO;
using BurgerRoyale.Domain.Entities;
using BurgerRoyale.Domain.Enumerators;
using BurgerRoyale.Domain.Exceptions;
using BurgerRoyale.Domain.Helpers;
using BurgerRoyale.Domain.Interface.Repositories;
using BurgerRoyale.Domain.Interface.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BurgerRoyale.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OrderDTO> CreateAsync(CreateOrderDTO orderDTO)
    {
        Order order = CreateOrder(orderDTO);

        await AddOrderProductsToOrder(orderDTO, order);

        order.SetOrderNumber(await GenerateOrderNumber());

        await _orderRepository.AddAsync(order);

        return new OrderDTO(order);
    }

    private Order CreateOrder(CreateOrderDTO orderDTO)
    {
        var userId = GetLoggedUserId();

        return new Order(userId);
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
            throw new NotFoundException("Produto(s) inválido(s).");
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

    public async Task<IEnumerable<OrderDTO>> GetOrdersAsync(OrderStatus? orderStatus)
    {
        var userId = IsCustomer() ? GetLoggedUserId() : null;

        var orders = await _orderRepository.GetOrders(orderStatus, userId);

        var orderDTOs = orders.Select(order => new OrderDTO(order)).ToList();

        return orderDTOs;
    }

    public async Task<OrderDTO> GetOrderAsync(Guid id)
    {
        var userId = IsCustomer() ? GetLoggedUserId() : null;

        var order = await _orderRepository.GetOrder(id, userId);

        ValidateIfOrderDoesNotExist(order);

        return new OrderDTO(order!);
    }

    public async Task RemoveAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        ValidateIfOrderDoesNotExist(order);

        _orderRepository.Remove(order!);
    }

    public async Task UpdateOrderStatusAsync(Guid id, OrderStatus orderStatus)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        ValidateIfOrderDoesNotExist(order);

        ValidateIfStatusIsTheSame(orderStatus, order);

        order!.SetStatus(orderStatus);

        await _orderRepository.UpdateAsync(order);
    }

    private static void ValidateIfOrderDoesNotExist(Order? order)
    {
        if (order is null)
            throw new DomainException("Pedido inválido.");
    }

    private static void ValidateIfStatusIsTheSame(OrderStatus orderStatus, Order order)
    {
        if (order.Status == orderStatus)
            throw new DomainException($"Pedido já possui status {orderStatus.GetDescription()}");
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
}
