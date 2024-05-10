using BurgerRoyale.Orders.Domain.Exceptions;
using BurgerRoyale.Orders.Domain.ResponseDefault;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace BurgerRoyale.Orders.API.ConfigController
{
	public class BaseController : ControllerBase
	{
		public IActionResult IStatusCode<T>([ActionResultObjectValue] T returnAPI) where T : ReturnAPI
		{
			var statusCode = GetStatus(returnAPI);

			if (statusCode != null)
			{
				var result = new ObjectResult(returnAPI) { StatusCode = (int)statusCode };

				return result;
			}

			throw new DomainException("Não foi informado StatusCode");
		}

		private static HttpStatusCode? GetStatus(object obj)
		{
			var type = obj.GetType();

			if (type != null && type.Name.StartsWith("ReturnAPI"))
			{
				return (HttpStatusCode?)type.GetProperty("StatusCode")?.GetValue(obj);
			}

			return null;
		}
	}
}
