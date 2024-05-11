namespace BurgerRoyale.Orders.Domain.DTO;

public record PaymentRequestDto
(
	Guid OrderId,
	decimal Amount,
	Guid? UserId
);
