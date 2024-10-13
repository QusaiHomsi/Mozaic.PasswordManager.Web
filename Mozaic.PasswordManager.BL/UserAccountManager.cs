using Mozaic.PasswordManager.DAL;
using Mozaic.PasswordManager.Entities;
using Mozaic.PasswordManager.Entities.SearchFilters;
using Mozaic.PasswordManager.Web.Models.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozaic.PasswordManager.BL
{
    public class UserAccountManager
    {
        public List<UserAccount> GetUserAccounts(UserAccountSearchFilter filter)
        {
            var repository = new UserAccountRepository();
            return repository.GetUserAccounts(filter); // Returns all matching accounts
        }
        public void CreateUserAccount(UserAccount userAccount)
        {
            var repository = new UserAccountRepository();
            repository.CreateUserAccount(userAccount); 
        }
        public UserAccount GetUserAccountById(int id)
        {
            var repository = new UserAccountRepository();
            return repository.GetUserAccountById(id);
        }
    }
}
