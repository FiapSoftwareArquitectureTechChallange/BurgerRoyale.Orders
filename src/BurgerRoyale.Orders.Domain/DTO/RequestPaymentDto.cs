namespace BurgerRoyale.Orders.Domain.DTO;

public record RequestPaymentDto
(
	Guid OrderId,
	decimal Amount,
	Guid? UserId
);
