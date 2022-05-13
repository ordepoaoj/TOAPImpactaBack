using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoAPImpacta.Areas.Identity.Data;
using ToDoAPImpacta.Models;

namespace ToDoAPImpacta.Interface
{
    public interface IRegister
    {
        Task<ToDoAPImpactaUser> RegisterUser(RegisterUser user, aspnetWebApplication253bc9b9d9d6a45d484292a2761773502Context _context, UserManager<IdentityUser> userManager);

        ToDoAPImpactaUser registerAdmin(aspnetWebApplication253bc9b9d9d6a45d484292a2761773502Context _context);
    }
}
