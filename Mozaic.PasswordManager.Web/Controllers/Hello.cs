using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize]
    public class GreetingController : Controller
    {
        // GET: /Greeting/Hello
        public IActionResult Hello()
        {
            var username = User.Identity.Name;
            ViewData["Username"] = username; // Pass the username to the view

            // Optional: Set a different message for admins
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
