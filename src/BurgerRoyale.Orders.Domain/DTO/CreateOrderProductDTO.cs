﻿namespace BurgerRoyale.Orders.Domain.DTO
{
	public class CreateOrderProductDTO
	{
		public Guid ProductId { get; set; }
		public int Quantity { get; set; }
	}
}
