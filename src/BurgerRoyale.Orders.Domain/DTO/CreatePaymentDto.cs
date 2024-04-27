namespace BurgerRoyale.Orders.Domain.DTO;

public record CreatePaymentDto
(
	decimal Amount,
	Guid? ClientIdentifier,
	string? CallbackUrl
);
