using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Actio.Common.Auth
{
    public static class Extensions
    {
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new JwtOptions();
            var section = configuration.GetSection("jwt");
            section.Bind(options);
            services.Configure<JwtOptions>(section);
            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddAuthentication()
            .AddJwtBearer(cfg=>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidIssuer=options.Issuer,
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey))
                };
            } );
        }
    }
}