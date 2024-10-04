using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mozaic.PasswordManager.Web.Models.DBEntities;
using Mozaic.PasswordManager.Web.Models.ViewModels;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RegisterController : Controller
    {
        private readonly AppDbContext _context;

        public RegisterController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Admin/Register.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(SystemUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                var newUser = new SystemUser
                {
                    UserName = model.UserName,
                    HashedPassword = hashedPassword,
                    IsAdmin = model.IsAdmin
                };

                _context.SystemUsers.Add(newUser);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "User successfully created!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Failed to create user. Please check the form.";
            return View("~/Views/Admin/Register.cshtml", model);
        }
    }
}
