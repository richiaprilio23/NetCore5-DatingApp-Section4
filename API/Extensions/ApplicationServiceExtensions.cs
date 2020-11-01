using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions //static class adalah tidak perlu membuat instance(inisialisasi) baru
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService , TokenService>(); //Req HTTP JWT, panggil class dan interface 
            services.AddDbContext<DataContext>(options => 
            { 
                 options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            }); 

            return services;
        }
    }
}