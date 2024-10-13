using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize]
    public class GreetingController : BaseController
    {
        public IActionResult Hello()
        {
            var username = GetUsernameFromHeaderOrIdentity();
            ViewData["Username"] = username;

            var userId = UserInformation?.Id;
            var userName = UserInformation?.UserName;

           
            if (IsUserAdmin())
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
