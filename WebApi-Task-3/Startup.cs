using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Task_3.DAL;
using WebApi_Task_3.DTOs.Books;
using WebApi_Task_3.DTOs.Categories;
using WebApi_Task_3.Mapping.Profiles;
using WebApi_Task_3.Models;

namespace WebApi_Task_3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<APIDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("default"));
            });
            
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<BooksPostDtoValidation>();
            //services.AddValidatorsFromAssemblyContaining<CategoryPostDtoValidation>();
            
            services.AddAutoMapper(opt =>
            {
                opt.AddProfile(new MapProfile());
            });
            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.User.AllowedUserNameCharacters = "qwertyuiopasdfghjkl;zxcfvgbhnm,1234567890QWERTYUIOPASDFGHJKLZXCVBNM<";
                opt.User.RequireUniqueEmail = true;

                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Lockout.AllowedForNewUsers = true;
            }).AddEntityFrameworkStores<APIDbContext>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(config =>
            {
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration.GetSection("Jwt:Issuer").Value,
                    ValidAudience = Configuration.GetSection("Jwt:Audience").Value,
                    IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Jwt:Key").Value)),
                    ClockSkew=TimeSpan.Zero
                    
                };
            });
        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
