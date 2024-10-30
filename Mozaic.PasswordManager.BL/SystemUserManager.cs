using Mozaic.PasswordManager.DAL;
using Mozaic.PasswordManager.Entities.SearchFilters;
using Mozaic.PasswordManager.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mozaic.PasswordManager.DAL.Exceptions;
using Mozaic.PasswordManager.BL.Exceptions;
using Serilog;
using Mozaic.PasswordManager.Common;

namespace Mozaic.PasswordManager.BL
{
    public class SystemUserManager
    {
        public List<SystemUser> GetSystemUser(SystemUserSearchFilter filter)
        {
            try
            {
                
                var repository = new SystemUserRepository();
                return repository.GetSystemUser(filter);
            }
            catch (Exception ex)
            {
                LogingHelper.LogError( "an error occured while getting system user",ex);
                throw new BusinessException("an error occured while getting system user", ex);
            }
        }

        public SystemUser GetSystemUserById(int id)
        {
            try
            {
                var repository = new SystemUserRepository();
                return repository.GetSystemUserById(id);
            }
            catch (Exception ex)
            {
                LogingHelper.LogError("an error occured while getting system user by id ", ex);
                throw new BusinessException("an error occured while getting system user by id", ex);
            }
        }

        public void UpdateUser(SystemUser user)
        {
            try
            {
                var repository = new SystemUserRepository();
                repository.UpdateUser(user);
            }
            catch (Exception ex)
            {
                LogingHelper.LogError("an error occured while updating system user ", ex);
                throw new BusinessException("an error occured while updating system user", ex);
            }
        }

        public SystemUser GetSystemUserByUserName(string username)
        {
            try
            {
                var repository = new SystemUserRepository();
                return repository.GetSystemUserByUserName(username);
            }
            catch (Exception ex)
            {
                LogingHelper.LogError("an error occured while updating system user ", ex);
                throw new BusinessException("an error occured while updating system user", ex);
            }
        }

        public async Task<TransactionResult> CreateUser(SystemUser user)
        {
            try
            {
                
                var repository = new SystemUserRepository();
                return await repository.CreateUser(user);

            }
            catch (Exception ex)
            {
                LogingHelper.LogError("an error occured while creating system user", ex);
                throw new BusinessException("an error occured while creating system user", ex);
            }
        }
    }
}
