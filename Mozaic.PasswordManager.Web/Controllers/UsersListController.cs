using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mozaic.PasswordManager.Web.Models.DBEntities;
using Mozaic.PasswordManager.Web.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SystemUserController : Controller
    {
        private readonly AppDbContext _context;

        public SystemUserController(AppDbContext context)
        {
            _context = context;
        }

        // Action to display all users
        public async Task<IActionResult> Users()
        {
            var users = await _context.SystemUsers
                .Select(u => new SystemUserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    IsAdmin = u.IsAdmin ?? false
                })
                .ToListAsync();

            return View("/Views/Admin/Users.cshtml", users);
        }

        // Action to display edit user form
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.SystemUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                IsAdmin = user.IsAdmin ?? false // Assign false if IsAdmin is null
            };

            return View(viewModel);
        }

        // POST action for saving edited user data
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Log validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(model); // Return to the same view with the model to show validation errors
            }

            var user = await _context.SystemUsers.FindAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            // Update user properties
            user.UserName = model.UserName;
            user.IsAdmin = model.IsAdmin;

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log exception details for troubleshooting
                Console.WriteLine($"An error occurred updating the user: {ex.Message}");
                return View(model); // Optionally return the view with the model
            }

            return RedirectToAction(nameof(Users)); // Redirect to the Users action
        }

        // Action for changing password
        public async Task<IActionResult> ChangePassword(int id)
        {
            var user = await _context.SystemUsers.FindAsync(id);
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

        // POST action for changing password
        [HttpPost]
        public async Task<IActionResult> ChangePassword(int id, string newPassword)
        {
            var user = await _context.SystemUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Hash the new password (using BCrypt for password hashing)
            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword); // Ensure you update the correct property for the hashed password

            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Users)); // Redirect to the Users action
        }
    }
}
