using BurgerRoyale.Orders.Infrastructure.HostedServices.SQSBackgroundServices;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace BurgerRoyale.Orders.IOC.Configurations
{
    [ExcludeFromCodeCoverage]
	public static class ConfigureHostedServices
    {
		public static void Register
		(
			IServiceCollection services
		)
		{
            services.AddHostedService<OrderPaymentResponseBackgroundService>();
        }
	}
}
