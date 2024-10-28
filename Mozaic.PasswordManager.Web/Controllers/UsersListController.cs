using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mozaic.PasswordManager.BL;
using Mozaic.PasswordManager.Entities;
using Mozaic.PasswordManager.Entities.SearchFilters;
using Mozaic.PasswordManager.Web.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SystemUserController : BaseController
    {
        private readonly IMapper _mapper;

        public SystemUserController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ActionResult>  Users(SystemUserSearchFilter filter)
        {


            int zero = 0;
            int result = 100 / zero;
            var manager = new SystemUserManager();
                var users =  manager.GetSystemUser(filter);
                var viewModel = _mapper.Map<List<SystemUserViewModel>>(users);
                return View("/Views/Admin/Users.cshtml", viewModel);
            
            
        }

        public IActionResult Edit(int id)
        {
            var manager = new SystemUserManager();
            var user = manager.GetSystemUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<EditUserViewModel>(user);
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

            _mapper.Map(model, user);
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

            var viewModel = _mapper.Map<SystemUserViewModel>(user);
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
