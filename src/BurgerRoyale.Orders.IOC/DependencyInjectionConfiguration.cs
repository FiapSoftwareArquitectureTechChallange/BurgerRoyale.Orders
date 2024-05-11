using BurgerRoyale.Orders.IOC.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace BurgerRoyale.Orders.IOC
{
    [ExcludeFromCodeCoverage]
	public static class DependencyInjectionConfiguration
	{
		public static void Register
		(
			IServiceCollection services,
			IConfiguration configuration
		)
		{
			ConfigureDatabase.Register(services, configuration);
			ConfigureHealthChecks.Register(services);
			ConfigureOptions.Register(services, configuration);
			ConfigureSecurity.Register(services, configuration);
            ConfigureMessageService.Register(services, configuration);
            ConfigureServices.Register(services);
            ConfigureHostedServices.Register(services);
        }
	}
}