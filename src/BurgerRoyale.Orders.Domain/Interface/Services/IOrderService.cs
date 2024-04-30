using BurgerRoyale.Orders.Domain.DTO;
using BurgerRoyale.Orders.Domain.Enumerators;

namespace BurgerRoyale.Orders.Domain.Interface.Services
{
	public interface IOrderService
	{
		Task<OrderDTO> CreateAsync(CreateOrderDTO orderDTO);

		Task<OrderDTO> GetOrderAsync(Guid id);

		Task<IEnumerable<OrderDTO>> GetOrdersAsync(OrderStatus? orderStatus);

		Task UpdateOrderStatusAsync(Guid id, OrderStatus orderStatus);

		Task RemoveAsync(Guid id);

		Task<int> GenerateOrderNumber();

		Task UpdatePaymentStatusAsync(Guid id, bool paymentSuccesfullyProcessed);
	}
}