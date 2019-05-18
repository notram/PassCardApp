using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


//
//https://forums.asp.net/t/2151809.aspx?MVC+Core+2+1+Seeding+Users+Roles+Issue
//

namespace PassCardApp
{
    public class AppSeed
    {
        public static async Task SeedAsync(IServiceProvider SvcProv)
        {
            var adminEmail = "admin@s.d";
            var cashierEmail = "cashier@s.d";
            var password = "qwer1234QWER!@#$";
            var UserManager = SvcProv.GetRequiredService<UserManager<IdentityUser>>();
            var RoleManager = SvcProv.GetRequiredService<RoleManager<IdentityRole>>();
            string[] RoleNames = { "Admin", "Cashier" };
            IdentityResult RoleResult;

            foreach (var RoleName in RoleNames)
            {
                var RoleExists = await RoleManager.RoleExistsAsync(RoleName);

                if (!RoleExists)
                {
                    RoleResult = await RoleManager.CreateAsync(new IdentityRole(RoleName));
                }
            }
            //add admin
            if (UserManager.FindByEmailAsync(adminEmail).Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    Email = adminEmail,
                    NormalizedEmail = adminEmail,
                    EmailConfirmed = true,
                    UserName = adminEmail,
                    NormalizedUserName = adminEmail,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false
                };

                IdentityResult result = UserManager.CreateAsync(user, password).Result;

                if (result.Succeeded)
                {
                    UserManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
            //add cashier
            if (UserManager.FindByEmailAsync(cashierEmail).Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    Email = cashierEmail,
                    NormalizedEmail = cashierEmail,
                    EmailConfirmed = true,
                    UserName = cashierEmail,
                    NormalizedUserName = cashierEmail,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false
                };

                IdentityResult result = UserManager.CreateAsync(user, password).Result;

                if (result.Succeeded)
                {
                    UserManager.AddToRoleAsync(user, "Cashier").Wait();
                }
            }
        }
    }
}
