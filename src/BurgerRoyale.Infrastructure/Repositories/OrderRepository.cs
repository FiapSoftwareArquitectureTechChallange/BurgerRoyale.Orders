using BurgerRoyale.Domain.Entities;
using BurgerRoyale.Domain.Enumerators;
using BurgerRoyale.Domain.Interface.Repositories;
using BurgerRoyale.Infrastructure.Context;
using BurgerRoyale.Infrastructure.RepositoriesStandard;
using Microsoft.EntityFrameworkCore;

namespace BurgerRoyale.Infrastructure.Repositories
{
    public class OrderRepository : DomainRepository<Order>, IOrderRepository
	{
		public OrderRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
		{
		}

		public async Task<Order?> GetOrder(Guid id, Guid? userId)
		{
			return await _context.Orders
				.Include(x => x.OrderProducts)
				.ThenInclude(x => x.Product)
				.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
		}

		public async Task<IEnumerable<Order>> GetOrders(OrderStatus? orderStatus, Guid? userId)
		{
			var query = _context.Orders.AsQueryable();

			query = (orderStatus == null)
				? query.Where(x => x.Status != OrderStatus.Finalizado)
				: query.Where(x => x.Status == orderStatus);

			if (userId != null)
			{
				query = query.Where(x => x.UserId == userId);
			}

			return await query
				.Include(x => x.OrderProducts)
				.ThenInclude(x => x.Product)
				.OrderByDescending(x => x.Status)
				.ThenBy(x => x.OrderTime)
				.ToListAsync();
		}
	}
}
