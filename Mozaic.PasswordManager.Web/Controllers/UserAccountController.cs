using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mozaic.PasswordManager.Entities;
using Mozaic.PasswordManager.DAL;
using Mozaic.PasswordManager.Web.Models.ViewModels;
using Mozaic.PasswordManager.Entities.SearchFilters;
using Mozaic.PasswordManager.BL;
using AutoMapper;

namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize("User")]
    public class UserAccountController : BaseController
    {
        private readonly IMapper _mapper;

        public UserAccountController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPasswords(string accountName, string userName)
        {
            var manager = new UserAccountManager();
            var filter = new UserAccountSearchFilter
            {
                Id = UserInformation.Id,
                UserName = userName,
                Name = accountName,
            };
           
            var userAccounts = manager.GetUserAccounts(filter);
            var viewModel = _mapper.Map<List<UserAccountViewModel>>(userAccounts);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserAccountViewModel());
        }

        [HttpPost]
        public IActionResult Create(UserAccountViewModel model)
        {
            var systemUserManager = new SystemUserManager();
            var systemUser = systemUserManager.GetSystemUserById(UserInformation.Id);

            if (ModelState.IsValid)
            {
                if (systemUser == null)
                {
                    return NotFound("User not found.");
                }

                var encryptedPassword = SymmetricEncryption.Encrypt(model.Password, model.EncryptionKey);
                var userAccount = _mapper.Map<UserAccount>(model);
                userAccount.UserId = systemUser.Id;
                userAccount.Password = encryptedPassword;

                var userAccountManager = new UserAccountManager();
                userAccountManager.CreateUserAccount(userAccount);

                return RedirectToAction("GetPasswords");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult DecryptPassword(int id, string encryptionKey)
        {
            var manager = new UserAccountManager();
            var userAccount = manager.GetUserAccountById(id);
            if (userAccount == null)
            {
                return NotFound("User account not found.");
            }

            var decryptedPassword = SymmetricEncryption.Decrypt(userAccount.Password, encryptionKey);
            return Json(new { decryptedPassword });
        }
    }
}
