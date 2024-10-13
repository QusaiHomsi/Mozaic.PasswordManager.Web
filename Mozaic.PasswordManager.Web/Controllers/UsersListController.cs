using Microsoft.AspNetCore.Mvc;
using Mozaic.PasswordManager.Web.Models.ViewModels;
using Mozaic.PasswordManager.DAL;
using Mozaic.PasswordManager.Entities.SearchFilters;
using Mozaic.PasswordManager.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Mozaic.PasswordManager.BL;


namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SystemUserController : BaseController
    {
       
        public IActionResult Users(SystemUserSearchFilter filter)
        {
            var manager = new SystemUserManager();
            var users = manager.GetSystemUser(filter)
                .Select(u => new SystemUserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    IsAdmin = u.IsAdmin ?? false
                }).ToList();

            return View("/Views/Admin/Users.cshtml", users);
        }

        public IActionResult Edit(int id)
        {
            var manager = new SystemUserManager();
            var user = manager.GetSystemUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                IsAdmin = user.IsAdmin ?? false
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var manager = new SystemUserManager();
            var user = manager.GetSystemUserById(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = model.UserName;
            user.IsAdmin = model.IsAdmin;

            manager.UpdateUser(user);

            return RedirectToAction(nameof(Users));
        }

        public IActionResult ChangePassword(int id)
        {
            var manager = new SystemUserManager();
            var user = manager.GetSystemUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new SystemUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ChangePassword(int id, string newPassword)
        {
            var manager = new SystemUserManager();
            var user = manager.GetSystemUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            user.password = BCrypt.Net.BCrypt.HashPassword(newPassword);

            manager.UpdateUser(user);

            return RedirectToAction(nameof(Users));
        }
    }
}
