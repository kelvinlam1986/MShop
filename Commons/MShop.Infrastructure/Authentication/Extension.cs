using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MShop.Infrastructure.Authentication
{
    public static class Extension
    {
        public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration) 
        {
            var options = new JwtOptions();
            configuration.GetSection("jwt").Bind(options);
            services.Configure<JwtOptions>(x => x = options);
            services.AddSingleton<IAuthenticationHandler, AuthenticationHandler>();
            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidIssuer = options.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey))
                    };
                });
            return services;
        }
    }
}
