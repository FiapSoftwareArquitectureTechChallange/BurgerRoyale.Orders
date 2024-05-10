using BurgerRoyale.Orders.Domain.Entities;
using BurgerRoyale.Orders.Domain.Enumerators;
using BurgerRoyale.Orders.Domain.Interface.Repositories;
using BurgerRoyale.Orders.Infrastructure.Context;
using BurgerRoyale.Orders.Infrastructure.RepositoriesStandard;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;

namespace BurgerRoyale.Orders.Infrastructure.Repositories
{
	[ExcludeFromCodeCoverage]
    public class OrderRepository(ApplicationDbContext applicationDbContext) : DomainRepository<Order>(applicationDbContext), IOrderRepository
	{
        public async Task<Order?> GetOrder(Guid id, Guid? userId)
		{
			return await GetOrderQuery()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
		}

        public async Task<Order?> GetOrder(Guid id)
        {
            return await GetOrderQuery()
				.FirstOrDefaultAsync(x => x.Id == id);
        }

        private IIncludableQueryable<Order, Product?> GetOrderQuery()
        {
            return _context.Orders
                .Include(x => x.OrderProducts)
                .ThenInclude(x => x.Product);
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
