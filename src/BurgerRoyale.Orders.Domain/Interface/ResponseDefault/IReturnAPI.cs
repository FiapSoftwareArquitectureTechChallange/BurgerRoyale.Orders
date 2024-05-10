using System.Net;

namespace BurgerRoyale.Orders.Domain.Interface.ResponseDefault
{
	public interface IReturnAPI
	{
		bool IsSuccessStatusCode { get; }
		bool IsNoContentStatusCode { get; }
		HttpStatusCode StatusCode { get; }
		string Message { get; }
		Exception Exception { get; }
		Dictionary<string, string[]> ModelState { get; }
	}

	public interface IReturnAPI<out TData>
	{
		TData Data { get; }
	}
}
