using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mozaic.PasswordManager.Web.Models.ViewModels;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mozaic.PasswordManager.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Mozaic.PasswordManager.Entities;
using Mozaic.PasswordManager.BL;


namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize]
    public class LoginController : BaseController
    {
        private readonly JwtConfig _jwtConfig;

        public LoginController(IOptions<JwtConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig.Value;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View(new SystemUserViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Index(SystemUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var manager = new SystemUserManager();
                var user = manager.GetSystemUserByUserName(model.UserName);

                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.password))
                {
                    var token = GenerateJwtToken(user);
                    Response.Cookies.Append("JWT", token);

                    return RedirectToAction("Hello", "Greeting");
                }

                ModelState.AddModelError(string.Empty, "Invalid username or password.");
            }

            return View(model);
        }

        private string GenerateJwtToken(SystemUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.IsAdmin.GetValueOrDefault() ? "Admin" : "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_jwtConfig.Issuer, _jwtConfig.Audience, claims, expires: DateTime.UtcNow.AddDays(_jwtConfig.ExpireDays), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
