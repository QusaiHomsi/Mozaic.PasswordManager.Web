using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize]
    public class GreetingController : BaseController
    {
        public IActionResult Hello()
        {
            ViewData["Username"] = UserInformation?.UserName;
            ViewData["RoleMessage"] = IsUserAdmin()
                ? "Admin Dashboard You Can Create SystemUsers."
                : "User Dashboard You Can Add User Accounts.";

            return View();
        }
    }
}
