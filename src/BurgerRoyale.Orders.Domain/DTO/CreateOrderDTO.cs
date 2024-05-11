namespace BurgerRoyale.Orders.Domain.DTO
{
	public class CreateOrderDTO
	{
		public IEnumerable<CreateOrderProductDTO> OrderProducts { get; set; } = Enumerable.Empty<CreateOrderProductDTO>();
	}
}