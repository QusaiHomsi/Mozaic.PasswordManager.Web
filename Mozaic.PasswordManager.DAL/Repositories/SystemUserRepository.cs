using Microsoft.EntityFrameworkCore;
using Mozaic.PasswordManager.Entities.SearchFilters;
using Mozaic.PasswordManager.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Mozaic.PasswordManager.DAL;
using Mozaic.PasswordManager.DAL.Exceptions;



namespace Mozaic.PasswordManager.DAL
{
    internal class SystemUserRepository : RepositoryBase
    {
        public List<SystemUser> GetSystemUser(SystemUserSearchFilter filter)
        {
            try
            {
                return SystemUserProvider.GetSystemUser(filter);
            }
            catch (Exception ex)
            {

                throw new DataProviderException("DATABASE EXCEPTION WHILE OCCURED WHILE Getting System USER", ex);
            }
        }

        public SystemUser GetSystemUserById(int id)
        {
            try
            {
                return AppDbContext.SystemUsers.FirstOrDefault(u => u.Id == id);
            }
            catch (Exception ex)
            {

                throw new DataProviderException("DATABASE EXCEPTION WHILE OCCURED WHILE Getting System USER by ID", ex);
            }
        }

        public void UpdateUser(SystemUser user)
        {
            try
            {
                AppDbContext.SystemUsers.Update(user);
                AppDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new DataProviderException("DATABASE EXCEPTION WHILE OCCURED WHILE Updating System User", ex);
            }
        }

        public SystemUser GetSystemUserByUserName(string username)
        {
            try
            {
                return AppDbContext.SystemUsers.FirstOrDefault(u => u.UserName == username);
            }
            catch (Exception ex)
            {

                throw new DataProviderException("DATABASE EXCEPTION WHILE OCCURED WHILE Getting System USER by username", ex);
            }
        }

        public async Task CreateUser(SystemUser user)
        {
            try
            {
                await SystemUserProvider.CreateUser(user);
            }
            catch (Exception ex)
            {

                throw new DataProviderException("DATABASE EXCEPTION WHILE OCCURED WHILE CREATE USER",ex);
            }
        }
    }

}

