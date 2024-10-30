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
                var password = BCrypt.Net.BCrypt.HashPassword(model.Password);
        

                var newUser = _mapper.Map<SystemUser>(model);
                newUser.password = password;
                newUser.CreationDate = DateTime.UtcNow;
                newUser.CreatedBy = this.UserInformation.Id; 
          

                var result = await manager.CreateUser(newUser);
                ViewBag.Message = " User has been created successfully";
                if (result.Status != SystemTransactionStatus.Success)
                {
                    ViewBag.Message = result.Message;
                    return View("~/Views/Admin/Register.cshtml", model);
                }


                return RedirectToAction("Users", "SystemUser");

            }

            ViewBag.Message = "Failed to create user. Please check the form.";
            return View("~/Views/Admin/Register.cshtml", model);
        }
    }
}
