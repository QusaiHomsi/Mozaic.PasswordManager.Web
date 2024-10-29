using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mozaic.PasswordManager.Entities;
using Mozaic.PasswordManager.DAL;
using Mozaic.PasswordManager.Web.Models.ViewModels;
using Mozaic.PasswordManager.Entities.SearchFilters;
using Mozaic.PasswordManager.BL;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mozaic.PasswordManager.Web.Models.DBEntities;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize("User")]
    public class UserAccountController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public UserAccountController(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUserAccounts(string accountName, string userName, int pageNumber = 1, int pageSize = 5)
        {
            var manager = new UserAccountManager();
            var filter = new UserAccountSearchFilter
            {
                Id = UserInformation.Id,
                UserName = userName,
                Name = accountName,
                PageNumber = pageNumber,
                PageSize = pageSize // Use the page size passed in the parameters
            };

            var userAccounts = manager.GetUserAccounts(filter);

            var totalCount = _context.UserAccounts.Count(ua => ua.UserId == UserInformation.Id &&
                (string.IsNullOrWhiteSpace(filter.Name) || ua.AccountName.Contains(filter.Name)) &&
                (string.IsNullOrWhiteSpace(filter.UserName) || ua.UserName.Contains(filter.UserName)));

            var viewModel = _mapper.Map<List<UserAccountViewModel>>(userAccounts);

            ViewBag.TotalCount = totalCount;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize; // Set the current page size in ViewBag

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
                userAccount.CreationDate = DateTime.UtcNow;

                var userAccountManager = new UserAccountManager();
                userAccountManager.CreateUserAccount(userAccount);

                return RedirectToAction("GetUserAccounts");
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
                return Json(new { decryptedPassword });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}
