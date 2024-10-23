using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mozaic.PasswordManager.BL;
using Mozaic.PasswordManager.Entities;
using Mozaic.PasswordManager.Web.Models.ViewModels;
using System;
using BCrypt.Net;
using Serilog;
using Mozaic.PasswordManager.BL.Exceptions;


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
                try
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
                catch (BusinessException ex) 
                {
                    TempData["ErrorMessage"] = $"An error occurred while creating the user: {ex.Message}";
                    return View("~/Views/Admin/Register.cshtml", model);
                }
                catch (Exception ex) 
                {
                    TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
                    Log.Error(ex, "An unexpected error occurred while creating a user."); 
                    return View("~/Views/Admin/Register.cshtml", model);
                }
            }

            TempData["ErrorMessage"] = "Failed to create user. Please check the form.";
            return View("~/Views/Admin/Register.cshtml", model);
        }

    }
}
    