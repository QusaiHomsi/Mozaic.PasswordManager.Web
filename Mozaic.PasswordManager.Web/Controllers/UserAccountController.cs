using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mozaic.PasswordManager.Web.Models;
using Mozaic.PasswordManager.Web.Models.DBEntities;
using Mozaic.PasswordManager.Web.Models.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens; 

namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize("User")]
    public class UserAccountController : Controller
    {
        private readonly AppDbContext _context;

        public UserAccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListPasswords()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            if (userIdClaim == null)
            {
                return NotFound("User not found.");
            }

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            var userAccounts = await _context.UserAccounts
                .Where(ua => ua.UserId == userId)
                .ToListAsync();

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
        public async Task<IActionResult> Create(UserAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                var systemUser = await _context.SystemUsers.FindAsync(int.Parse(userIdClaim));

                if (systemUser == null)
                {
                    return NotFound("User not found.");
                }

                var encryptedPassword = SymmetricEncryption.Encrypt(model.Password, systemUser.HashedPassword);

                var userAccount = new UserAccount
                {
                    AccountName = model.AccountName,
                    UserName = model.UserName,
                    Password = encryptedPassword, 
                    UserId = systemUser.Id
                };

                _context.UserAccounts.Add(userAccount);
                await _context.SaveChangesAsync();

                return RedirectToAction("ListPasswords");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewPassword(int id)
        {
            var userAccount = await _context.UserAccounts.FindAsync(id);
            if (userAccount == null)
            {
                return NotFound("User account not found.");
            }

            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            var systemUser = await _context.SystemUsers.FindAsync(int.Parse(userIdClaim));
            if (systemUser == null)
            {
                return NotFound("User not found.");
            }

            var decryptedPassword = SymmetricEncryption.Decrypt(userAccount.Password, systemUser.HashedPassword);

            ViewBag.DecryptedPassword = decryptedPassword;

            return View(userAccount); 
        }
    }
}
