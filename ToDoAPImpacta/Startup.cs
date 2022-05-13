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
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoAPImpacta.Data;
using ToDoAPImpacta.Interface;
using ToDoAPImpacta.Models;
using ToDoAPImpacta.Services;

namespace ToDoAPImpacta
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
            services.AddDbContext<aspnetWebApplication253bc9b9d9d6a45d484292a2761773502Context>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("ToDoAPImpactaContextConnection")));
            services.AddMvc();
            services.AddControllers();
            services.AddRazorPages();

            services.AddScoped<IJobFactory, JobFactory>();
            services.AddScoped<IRegister, Register>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDoAPImpacta", Version = "v1" });
            });

            var key = Encoding.ASCII.GetBytes("fCrjDtJsUFNMOTqgAUxRHlDlv9ss9xZas46lc6Gu");

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    RequireExpirationTime = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Jwt:key"])),
                    ValidateIssuerSigningKey = true,

                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseStatusCodePages();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoAPImpacta v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //CreateRoles(serviceProvider).Wait();
            //CreateAdmin(serviceProvider).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[] rolesNames = { "Administrator", "User" };
            IdentityResult result;
            foreach (var namesRole in rolesNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(namesRole);
                if (!roleExist)
                {
                    result = await roleManager.CreateAsync(new IdentityRole(namesRole));
                }
            }
        }

        private async Task CreateAdmin(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var admin = await userManager.FindByNameAsync("admin@admin.com");

            if(admin == null)
            {
                var newUser = new IdentityUser() { Email = "admin@admin.com", EmailConfirmed = true, UserName = "admin@admin.com" };
                await userManager.CreateAsync(newUser, "Admin@2022");
                await userManager.AddToRoleAsync(newUser, "Administrator");
            }
        }
    }
}
