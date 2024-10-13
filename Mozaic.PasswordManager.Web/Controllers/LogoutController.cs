using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Mozaic.PasswordManager.Web.Controllers
{
    public class LogoutController : BaseController
    {
        [HttpPost]
        public IActionResult Logout()
        {
            
            Response.Cookies.Delete("JWT");

          
            HttpContext.SignOutAsync();

            
            return RedirectToAction("Index", "Login");
        }
    }
}
