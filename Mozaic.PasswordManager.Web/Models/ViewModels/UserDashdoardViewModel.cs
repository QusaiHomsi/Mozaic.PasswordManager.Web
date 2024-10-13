using Mozaic.PasswordManager.Entities;
using Mozaic.PasswordManager.Web.Models.DBEntities;
using System.Collections.Generic;

namespace Mozaic.PasswordManager.Web.Models.ViewModels
{
    public class UserDashboardViewModel
    {
        public string UserName { get; set; }
        public List<UserAccount> Accounts { get; set; }
    }
}
