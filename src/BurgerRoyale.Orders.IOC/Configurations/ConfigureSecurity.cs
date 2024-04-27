using BurgerRoyale.Orders.Domain.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace BurgerRoyale.Orders.IOC.Configurations
{
    [ExcludeFromCodeCoverage]
    public static class ConfigureSecurity
    {
        public static void Register
        (
            IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<JwtConfiguration>
            (
                options => configuration.GetSection("Jwt").Bind(options)
            );

            var jwtConfig = configuration
                .GetSection("Jwt")
                .Get<JwtConfiguration>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = jwtConfig?.Issuer,
                        ValidAudience = jwtConfig?.Audience,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey!))
                    };
                });
        }
    }
}
