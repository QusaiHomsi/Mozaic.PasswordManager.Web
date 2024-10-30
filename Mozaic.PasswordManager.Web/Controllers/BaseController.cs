using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Mozaic.PasswordManager.Web.Controllers
{
    public class UserInformation
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }

    public class BaseController : Controller
    {
        private UserInformation _UserInformation;

        public UserInformation UserInformation
        {
            get
            {
                if (_UserInformation == null)
                {
                    var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                    if (userIdClaim != null)
                    {
                        _UserInformation = new UserInformation
                        {
                            Id = int.Parse(userIdClaim),
                            UserName = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value

                        };
                    }
                }
                return _UserInformation;
            }
        }

        public bool IsUserAdmin()
        {
            return User.IsInRole("Admin");
        }
    }
}
