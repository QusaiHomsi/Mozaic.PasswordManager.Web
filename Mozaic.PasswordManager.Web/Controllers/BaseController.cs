using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Mozaic.PasswordManager.Web.Controllers
{
    public class UserInformation
    {
        public int Id { get; set; }
        public string  UserName{ get; set; }
    }
    public class BaseController: Controller
    {
        private UserInformation _UserInformation = null;
        public UserInformation UserInformation
        {

            get
            {


                if (_UserInformation == null)
                {

                    var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                    if (userIdClaim != null)
                    {
                        _UserInformation = new UserInformation();
                        _UserInformation.Id = int.Parse(userIdClaim);
                        _UserInformation.UserName = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                    }
                }

                return _UserInformation;
            }
        }

            public string GetUsernameFromHeaderOrIdentity()
        {
            var username = Request.Headers["X-Username"].FirstOrDefault();
            if (string.IsNullOrEmpty(username))
            {
                username = User.Identity.Name;
            }
            return username;
        }
        public bool IsUserAdmin()
        {
            return User.IsInRole("Admin");
        }
        protected string GetUserId()
        {
            return User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        }

        protected string GetUserName()
        {
            return User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        }


        public BaseController()
        {


          
            
        }
    }
}
