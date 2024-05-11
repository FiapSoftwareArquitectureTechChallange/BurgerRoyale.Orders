namespace BurgerRoyale.Orders.Domain.DTO;

public record PaymentFeedbackDto
(
	Guid OrderId,
	bool ProcessedSuccessfully
);
