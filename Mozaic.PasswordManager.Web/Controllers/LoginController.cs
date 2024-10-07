using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mozaic.PasswordManager.Web.Models.DBEntities;
using Mozaic.PasswordManager.Web.Models.ViewModels;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Mozaic.PasswordManager.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace Mozaic.PasswordManager.Web.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;
        private readonly JwtConfig _jwtConfig;

        public LoginController(AppDbContext context, IOptions<JwtConfig> jwtConfig)
        {
            _context = context;
            _jwtConfig = jwtConfig.Value;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View(new SystemUserViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(SystemUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"Searching for user: {model.UserName}");

                var user = await _context.SystemUsers
                    .FirstOrDefaultAsync(u => u.UserName == model.UserName);

                if (user != null)
                {
                    Console.WriteLine($"User found: {user.UserName}");

                    if (BCrypt.Net.BCrypt.Verify(model.Password, user.HashedPassword))
                    {
                        var token = GenerateJwtToken(user);

                        Response.Cookies.Append("JWT", token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = HttpContext.Request.IsHttps,
                            SameSite = SameSiteMode.Strict
                        });

                        return RedirectToAction("Hello", "Greeting"); 
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid password.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return View(model);
        }

        private string GenerateJwtToken(SystemUser user)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Log the username being used for the token
                Console.WriteLine($"Creating JWT for username: {user.UserName}");

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName), 
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()), 
                    new Claim(ClaimTypes.Role, user.IsAdmin.GetValueOrDefault() ? "Admin" : "User") 
                };

                var token = new JwtSecurityToken(
                    issuer: _jwtConfig.Issuer,
                    audience: _jwtConfig.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(_jwtConfig.ExpireDays),
                    signingCredentials: creds);

                Console.WriteLine($"Generated JWT for user: {user.UserName}");

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating JWT: {ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize("User")]
        public IActionResult MainPage()
        {
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value });
            Console.WriteLine("User Claims: " + string.Join(", ", userClaims));

            // Print logged-in user's ID and username
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value; 
            var userNameClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value; 

            Console.WriteLine($"Logged in User ID: {userIdClaim}");
            Console.WriteLine($"Logged in Username: {userNameClaim}"); 

            if (User.IsInRole("Admin"))
            {
                ViewBag.Message = "Welcome Admin!";
            }
            else
            {
                ViewBag.Message = "Welcome User!";
            }

            return View();
        }
    }
}
