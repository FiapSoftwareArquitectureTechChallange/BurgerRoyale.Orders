﻿using BurgerRoyale.Orders.Domain.Entities;
using BurgerRoyale.Orders.Domain.Enumerators;
using BurgerRoyale.Orders.Domain.Exceptions;
using Xunit;

namespace BurgerRoyale.Orders.UnitTests.Domain.Validation
{
	public class ProductShould
	{
		[Fact]
		public void Validate_When_Does_Not_Have_Name()
		{
			#region Act(When)

			DomainException result = Assert.Throws<DomainException>(() => new Product("", "", 0, ProductCategory.Lanche));

			#endregion Act(When)

			#region Assert(Then)

			Assert.Equal("The name is required!", result.Message);

			#endregion Assert(Then)
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		public void Validate_When_Does_Not_Have_Price(decimal price)
		{
			#region Act(When)

			DomainException result = Assert.Throws<DomainException>(() => new Product("Name", "", price, ProductCategory.Lanche));

			#endregion Act(When)

			#region Assert(Then)

			Assert.Equal("The price is invalid!", result.Message);

			#endregion Assert(Then)
		}
	}
}