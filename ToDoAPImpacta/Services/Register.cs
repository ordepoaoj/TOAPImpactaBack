using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoAPImpacta.Areas.Identity.Data;
using ToDoAPImpacta.Interface;
using ToDoAPImpacta.Models;

namespace ToDoAPImpacta.Services
{
    public class Register : IRegister
    {
        public async Task<ToDoAPImpactaUser> RegisterUser(RegisterUser user, aspnetWebApplication253bc9b9d9d6a45d484292a2761773502Context _context, UserManager<IdentityUser> userManager)
        {
            var newUser = new ToDoAPImpactaUser { UserName = user.email, Email = user.email, EmailConfirmed = true, FirstName = user.firstName };
            var result =  await userManager.CreateAsync(newUser, user.password);

            if (result.Succeeded)
            {
                if (user.isAdministrator == true)
                {
                    var role = _context.AspNetRoles.Where(r => r.Name == "ADMINISTRATOR").First();
                    AspNetUserRole userRole = new AspNetUserRole()
                    {
                        UserId = newUser.Id,
                        RoleId = role.Id
                    };
                    _context.AspNetUserRoles.Update(userRole);
                    _context.SaveChanges();
                }
                return newUser;
            }
            return null;
        }

        public ToDoAPImpactaUser registerAdmin(aspnetWebApplication253bc9b9d9d6a45d484292a2761773502Context _context)
        {
            throw new NotImplementedException();
        }
    }
}
