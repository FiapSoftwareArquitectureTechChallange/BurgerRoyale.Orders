using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.SQS;
using BurgerRoyale.Orders.Domain.Configuration;
using BurgerRoyale.Orders.Domain.Interface.IntegrationServices;
using BurgerRoyale.Orders.Infrastructure.IntegrationServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace BurgerRoyale.Orders.IOC.Configurations
{
    [ExcludeFromCodeCoverage]
	public static class ConfigureMessageService
    {
		public static void Register
		(
			IServiceCollection services,
            IConfiguration configuration
        )
		{
            var awsConfiguration = configuration
                .GetSection("AWS")
                .Get<AwsConfiguration>();

            services.AddDefaultAWSOptions(new AWSOptions() {
                Credentials = new SessionAWSCredentials(
                    awsConfiguration?.AccessKey,
                    awsConfiguration?.SecretKey,
                    awsConfiguration?.SessionToken
                ),
                Region = RegionEndpoint.GetBySystemName(awsConfiguration?.Region)
            });

            services.AddAWSService<IAmazonSQS>(ServiceLifetime.Scoped);

            services.AddScoped<IMessageService, AwsSqsService>();
		}
	}
}
