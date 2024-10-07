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
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await _context.SystemUsers.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var accounts = _context.UserAccounts.Where(ua => ua.UserId == userId).ToList();

            var model = new UserDashboardViewModel
            {
                UserName = user.UserName,
                Accounts = accounts
            };

            return View(model);
        }
    }
}
