using System.ComponentModel;

namespace BurgerRoyale.Orders.Domain.Enumerators
{
	public enum ProductCategory
	{
		[Description("Lanche")]
		Lanche,

		[Description("Acompanhamento")]
		Acompanhamento,

		[Description("Bebida")]
		Bebida,

		[Description("Sobremesa")]
		Sobremesa
	}
}
