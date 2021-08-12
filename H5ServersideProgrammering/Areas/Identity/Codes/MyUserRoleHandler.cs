using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H5ServersideProgrammering.Areas.Identity.Codes
{
    public class MyUserRoleHandler
    {
        public async Task CreateRole(string user, string role, IServiceProvider _serviceProvider)
        {
            var RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = _serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            IdentityResult roleResult;

            var userRoleCheck = await RoleManager.RoleExistsAsync(role);

            if (!userRoleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole(role));
            }

            IdentityUser identityUser = await UserManager.FindByEmailAsync(user);
            await UserManager.AddToRoleAsync(identityUser, role);
        }
    }
}
