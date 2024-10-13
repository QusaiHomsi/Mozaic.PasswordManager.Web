using Mozaic.PasswordManager.DAL;
using Mozaic.PasswordManager.Entities.SearchFilters;
using Mozaic.PasswordManager.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mozaic.PasswordManager.BL
{
    public class SystemUserManager
    {
        public List<SystemUser> GetSystemUser(SystemUserSearchFilter filter)
        {
            var repository = new SystemUserRepository();
            return repository.GetSystemUser(filter);
        }

        public SystemUser GetSystemUserById(int id)
        {
            var repository = new SystemUserRepository();
            return repository.GetSystemUserById(id);
        }

        public void UpdateUser(SystemUser user)
        {
            var repository = new SystemUserRepository();
            repository.UpdateUser(user);
        }

        public SystemUser GetSystemUserByUserName(string username)
        {
            var repository = new SystemUserRepository();  
            return repository.GetSystemUserByUserName(username);
        }

        public async Task CreateUser(SystemUser user)
        {
            var repository = new SystemUserRepository();
            await repository.CreateUser(user);
        }
    }
}
