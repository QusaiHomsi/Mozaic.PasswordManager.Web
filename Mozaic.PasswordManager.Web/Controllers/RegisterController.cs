using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mozaic.PasswordManager.Entities;
using Mozaic.PasswordManager.DAL;
using Mozaic.PasswordManager.Web.Models.ViewModels;
using System;
using System.Linq;
using BCrypt.Net;
using Mozaic.PasswordManager.BL;

namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RegisterController : BaseController
    {
       

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Admin/Register.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var manager =new SystemUserManager();
                var existingUser = manager.GetSystemUserByUserName(model.UserName);

                if (existingUser != null)
                {
                    TempData["ErrorMessage"] = "Username already exists. Please choose a different one.";
                    return View("~/Views/Admin/Register.cshtml", model);
                }

                var password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                var createdByUserName = GetUsernameFromHeaderOrIdentity();

                var newUser = new SystemUser
                {
                    UserName = model.UserName,
                    password = password,
                    IsAdmin = model.IsAdmin,
                    CreationDate = DateTime.UtcNow,
                    CreatedBy = createdByUserName
                };

                manager.CreateUser(newUser);
                TempData["SuccessMessage"] = "User successfully created!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Failed to create user. Please check the form.";
            return View("~/Views/Admin/Register.cshtml", model);
        }
    }
}
