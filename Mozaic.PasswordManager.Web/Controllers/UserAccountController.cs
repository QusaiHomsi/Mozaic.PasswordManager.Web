using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mozaic.PasswordManager.Entities;
using Mozaic.PasswordManager.DAL;
using Mozaic.PasswordManager.Web.Models.ViewModels;
using System.Linq;
using Mozaic.PasswordManager.Entities.SearchFilters;
using Mozaic.PasswordManager.BL;

namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize("User")]
    public class UserAccountController : BaseController
    {



        [HttpGet]
        public async Task<IActionResult> ListPasswords(string accountName, string userName)
        {

            var manager = new UserAccountManager();
            

            var filter = new UserAccountSearchFilter
            {

                Id = UserInformation.Id,
                UserName = userName,
                Name = accountName,
            };
            var userAccounts = manager.GetUserAccounts(filter);

            var viewModel = userAccounts.Select(ua => new UserAccountViewModel
            {
                Id = ua.Id,
                AccountName = ua.AccountName,
                UserName = ua.UserName,
                Password = ua.Password
            }).ToList();

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
                var userAccount = new UserAccount
                {
                    AccountName = model.AccountName,
                    UserName = model.UserName,
                    Password = encryptedPassword,
                    UserId = systemUser.Id  
                };

                var userAccountManager = new UserAccountManager();
                userAccountManager.CreateUserAccount(userAccount);

                return RedirectToAction("ListPasswords");
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

            try
            {
                var decryptedPassword = SymmetricEncryption.Decrypt(userAccount.Password, encryptionKey);
                return Json(new { decryptedPassword = decryptedPassword });
            }
            catch (Exception)
            {
                return BadRequest("Invalid encryption key or error during decryption.");
            }
        }
    }
}
