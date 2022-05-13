using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoAPImpacta.Areas.Identity.Data;
using ToDoAPImpacta.Interface;
using ToDoAPImpacta.Models;

namespace ToDoAPImpacta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly aspnetWebApplication253bc9b9d9d6a45d484292a2761773502Context _context;
        private readonly IRegister _register;

        public RegisterController(UserManager<IdentityUser> userManager, aspnetWebApplication253bc9b9d9d6a45d484292a2761773502Context context, IRegister register)
        {
            _userManager = userManager;
            _context = context;
            _register = register;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUser user)
        {
            var newUser = new ToDoAPImpactaUser { UserName = user.email, Email = user.email, EmailConfirmed = true, FirstName = user.firstName };
            var result = await _userManager.CreateAsync(newUser, user.password);



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
                return Ok();
            }
            else
            {
                return StatusCode(403);
            }
        }
    }
}
