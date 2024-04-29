using BurgerRoyale.Orders.Application.Services;
using BurgerRoyale.Orders.Domain.Interface.IntegrationServices;
using BurgerRoyale.Orders.Domain.Interface.Repositories;
using BurgerRoyale.Orders.Domain.Interface.Services;
using BurgerRoyale.Orders.Infrastructure.IntegrationServices;
using BurgerRoyale.Orders.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace BurgerRoyale.Orders.IOC.Configurations
{
    [ExcludeFromCodeCoverage]
	public static class ConfigureServices
	{
		public static void Register
		(
			IServiceCollection services
		)
		{
			#region Services

			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IMessageService, AWSSQSService>();

            #endregion Services

            #region Repositories

            services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<IProductImageRepository, ProductImageRepository>();
			services.AddScoped<IOrderRepository, OrderRepository>();

			#endregion Repositories
		}
	}
}
