using BurgerRoyale.Orders.Domain.Entities;

namespace BurgerRoyale.Orders.UnitTests.Domain.EntitiesMocks
{
    public static class OrderMock
    {
		public static Order Get
		(
			Guid? userId = null
		)
		{
			return new Order(userId ?? Guid.NewGuid());
		}
	}
}
