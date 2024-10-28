using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mozaic.PasswordManager.BL;
using Mozaic.PasswordManager.Entities;
using Mozaic.PasswordManager.Web.Models.ViewModels;
using BCrypt.Net;

namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RegisterController : BaseController
    {
        private readonly IMapper _mapper;

        public RegisterController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Admin/Register.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var manager = new SystemUserManager();
                var existingUser = manager.GetSystemUserByUserName(model.UserName);

                if (existingUser != null)
                {
                    TempData["ErrorMessage"] = "Username already exists. Please choose a different one.";
                    return View("~/Views/Admin/Register.cshtml", model);
                }

                var password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                var createdByUserName = GetUsernameFromHeaderOrIdentity();

                var newUser = _mapper.Map<SystemUser>(model);
                newUser.password = password;
                newUser.CreationDate = DateTime.UtcNow;
                newUser.CreatedBy = createdByUserName;

                await manager.CreateUser(newUser);

                TempData["SuccessMessage"] = "User successfully created!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Failed to create user. Please check the form.";
            return View("~/Views/Admin/Register.cshtml", model);
        }
    }
}
