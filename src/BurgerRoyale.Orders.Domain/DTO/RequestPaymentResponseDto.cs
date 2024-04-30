namespace BurgerRoyale.Orders.Domain.DTO;

public record RequestPaymentResponseDto
(
	Guid OrderId,
	bool ProcessedSuccessfully
);
