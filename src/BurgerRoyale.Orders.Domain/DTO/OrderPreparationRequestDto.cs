namespace BurgerRoyale.Orders.Domain.DTO;

public record OrderPreparationRequestDto
(
	Guid OrderId,
    IEnumerable<OrderProductDTO> OrderProducts,
	Guid? UserId
);
