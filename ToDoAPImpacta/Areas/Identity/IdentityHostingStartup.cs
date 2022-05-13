using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoAPImpacta.Data;

[assembly: HostingStartup(typeof(ToDoAPImpacta.Areas.Identity.IdentityHostingStartup))]
namespace ToDoAPImpacta.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ToDoAPImpactaContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ToDoAPImpactaContextConnection")));

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ToDoAPImpactaContext>();
            });
        }
    }
}