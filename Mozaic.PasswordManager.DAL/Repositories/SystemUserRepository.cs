using Microsoft.EntityFrameworkCore;
using Mozaic.PasswordManager.Entities.SearchFilters;
using Mozaic.PasswordManager.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Mozaic.PasswordManager.DAL;



namespace Mozaic.PasswordManager.DAL
{
    internal class SystemUserRepository : RepositoryBase
    {
        public List<SystemUser> GetSystemUser(SystemUserSearchFilter filter)
        {
            return SystemUserProvider.GetSystemUser(filter);
        }

        public SystemUser GetSystemUserById(int id)
        {
            return AppDbContext.SystemUsers.FirstOrDefault(u => u.Id == id);
        }

        public void UpdateUser(SystemUser user)
        {
            AppDbContext.SystemUsers.Update(user);
            AppDbContext.SaveChanges();
        }

        public SystemUser GetSystemUserByUserName(string username)
        {
            return AppDbContext.SystemUsers.FirstOrDefault(u => u.UserName == username);
        }

        public async Task CreateUser(SystemUser user)
        {
            await SystemUserProvider.CreateUser(user);
        }
    }

}

