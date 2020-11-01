using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            // Configuration = config;
            _config = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // dicommen move ke Extensions > AddApplicationServices.cs
            services.AddApplicationServices(_config);
            // services.AddScoped<ITokenService , TokenService>(); //Req HTTP JWT, panggil class dan interface 
            // services.AddDbContext<DataContext>(options => 
            // { 
            //      options.UseNpgsql(_config.GetConnectionString("DefaultConnection"));
            // }); 
            

            services.AddControllers();
            services.AddCors(); //digunakan sby policy API MIddleware

            // dicommen move ke Extensions > AddIdentityServices.cs
            services.AddIdentityServices(_config);
            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //add sbg authentication middleware
            // .AddJwtBearer (options =>{
            //     options.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         //signature validasi server
            //         ValidateIssuerSigningKey = true,
            //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])), //TokenKey inisialisasi di dalam appsetings
            //         ValidateIssuer = false,
            //         ValidateAudience = false,
            //     };
            // });

             services.AddSwaggerGen(c =>
             {
                 c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
             });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x =>  //deklarasi policy API MIddleware
            x.AllowAnyHeader() //mengijikan header apapun
            .AllowAnyMethod() //mengijikan method apapun
            .WithOrigins("http://localhost:4200")); //asal mana mengijinkan permintaan API berasal

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
