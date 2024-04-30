namespace BurgerRoyale.Orders.Domain.DTO;

public record RequestOrderPreparationDto
(
	Guid OrderId,
    IEnumerable<OrderProductDTO> OrderProducts,
	Guid? UserId
);
