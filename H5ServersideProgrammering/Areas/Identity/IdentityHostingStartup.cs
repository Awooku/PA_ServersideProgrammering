using System;
using H5ServersideProgrammering.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(H5ServersideProgrammering.Areas.Identity.IdentityHostingStartup))]
namespace H5ServersideProgrammering.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<H5ServersideProgrammeringContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("H5ServersideProgrammeringContextConnection")));

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<H5ServersideProgrammeringContext>();

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("RequireAuthenticatedUser", policy => {
                        policy.RequireAuthenticatedUser();
                    });
                    options.AddPolicy("RequireAdminUser", policy => {
                        policy.RequireRole("Admin");
                    });
                });
            });
        }
    }
}