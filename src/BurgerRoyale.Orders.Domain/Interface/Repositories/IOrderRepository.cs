using BurgerRoyale.Orders.Domain.Entities;
using BurgerRoyale.Orders.Domain.Enumerators;
using BurgerRoyale.Orders.Domain.Interface.RepositoriesStandard;

namespace BurgerRoyale.Orders.Domain.Interface.Repositories
{
    public interface IOrderRepository : IDomainRepository<Order>
	{
		Task<IEnumerable<Order>> GetOrders(OrderStatus? orderStatus, Guid? userId);

		Task<Order?> GetOrder(Guid id, Guid? userId);

        Task<Order?> GetOrder(Guid id);
    }
}