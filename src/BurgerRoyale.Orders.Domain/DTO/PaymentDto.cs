namespace BurgerRoyale.Orders.Domain.DTO;

public record PaymentDto
(
	Guid PaymentId,
	decimal Amount,
	string Status,
	bool Paid,
	DateTime RequestedAt,
	DateTime? UpdatedAt
);