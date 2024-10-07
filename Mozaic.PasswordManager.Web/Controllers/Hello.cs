using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt; 
using System.Linq;

namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize]
    public class GreetingController : Controller
    {
        
        public IActionResult Hello()
        {
            var username = User.Identity.Name;
            ViewData["Username"] = username; 
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value });
            Console.WriteLine("User Claims: " + string.Join(", ", userClaims));

            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value; 
            var userNameClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value; 

            Console.WriteLine($"Logged in User ID: {userIdClaim}");
            Console.WriteLine($"Logged in Username: {userNameClaim}");
            if (User.IsInRole("Admin"))
            {
                ViewData["RoleMessage"] = "You have administrative privileges.";
            }
            else
            {
                ViewData["RoleMessage"] = "You are a regular user.";
            }

            return View();
        }
    }
}
