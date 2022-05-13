using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoAPImpacta.Models;

namespace ToDoAPImpacta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.LoginModel> _logger;
        private readonly IConfiguration _configuration;

        public LoginController(SignInManager<IdentityUser> signInManager, ILogger<Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.LoginModel> logger, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> SignInAsync([FromBody] Models.LoginModel loginModel)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.PassWord, false, lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(loginModel.Email);
                    return BuildToken(loginModel.Email, user);
                }
                else
                {
                    return NotFound("Usuário Não encontrado");
                }
            }
            return Ok();
        }
        private IActionResult BuildToken(string email, IdentityUser user)
        {
            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("Login", user.Email)
                   };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                                 _configuration["Jwt:Audience"], claims,
                                 expires: DateTime.UtcNow.AddMinutes(20), signingCredentials: signIn);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
