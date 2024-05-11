using BurgerRoyale.Orders.Domain.Exceptions;

namespace BurgerRoyale.Orders.Domain.Base
{
	public static class AssertionConcern
	{
		public static void AssertArgumentNotEmpty(string stringValue, string message)
		{
			if (stringValue == null || stringValue.Trim().Length == 0)
			{
				throw new DomainException(message);
			}
		}

		public static void AssertArgumentHasValidPrice(decimal price, string message)
		{
			if (price == 0 || price < 0)
			{
				throw new DomainException(message);
			}
		}
	}
}