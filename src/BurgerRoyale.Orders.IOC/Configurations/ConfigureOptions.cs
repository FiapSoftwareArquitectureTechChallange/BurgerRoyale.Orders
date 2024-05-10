using BurgerRoyale.Orders.Domain.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace BurgerRoyale.Orders.IOC.Configurations
{

    [ExcludeFromCodeCoverage]
    public static class ConfigureOptions
    {
        public static void Register
        (
            IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<AwsConfiguration>
            (
                options => configuration.GetSection("AWS").Bind(options)
            );

            services.Configure<MessageQueuesConfiguration>
            (
                options => configuration.GetSection("MessageQueues").Bind(options)
            );
        }
    }
}
