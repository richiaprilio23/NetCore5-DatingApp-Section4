using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions //static class adalah tidak perlu membuat instance (inisialisasi) baru
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config )
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //add sbg authentication middleware
            .AddJwtBearer (options =>{
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //signature validasi server
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])), //TokenKey inisialisasi di dalam appsetings
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
            return services;
        }
    }
}