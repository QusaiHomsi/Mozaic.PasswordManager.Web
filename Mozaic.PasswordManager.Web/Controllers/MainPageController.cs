using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mozaic.PasswordManager.Web.Models.DBEntities;
using Mozaic.PasswordManager.Web.Models.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize]
    public class MainPageController : Controller
    {
        private readonly AppDbContext _context;

        public MainPageController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Get the current logged-in user's ID from the claims
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Get the user details
            var user = await _context.SystemUsers.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Get the user's accounts
            var accounts = _context.UserAccounts.Where(ua => ua.UserId == userId).ToList();

            // Create the view model
            var model = new UserDashboardViewModel
            {
                UserName = user.UserName,
                Accounts = accounts
            };

            return View(model);
        }
    }
}
