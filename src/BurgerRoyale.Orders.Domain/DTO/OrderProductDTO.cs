using BurgerRoyale.Orders.Domain.Entities;

namespace BurgerRoyale.Orders.Domain.DTO
{
	public class OrderProductDTO
	{
		public Guid ProductId { get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }

        public OrderProductDTO(OrderProduct orderProduct)
        {
			ProductId = orderProduct.ProductId;
			ProductName = orderProduct.Product?.Name ?? string.Empty;
			Quantity = orderProduct.Quantity;
			Price = orderProduct.ProductPrice;
        }
    }
}