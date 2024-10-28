using Mozaic.PasswordManager.BL.Exceptions;
using Mozaic.PasswordManager.Common;
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
            try
            {
                var repository = new UserAccountRepository();
                return repository.GetUserAccounts(filter); 
            }
            catch (Exception ex)
            {
                //Log Exception
                LogingHelper.LogError("an error occured while getting User accounts", ex);
                throw new BusinessException("an error occured while getting User accounts ", ex);
            }
        }
        public void CreateUserAccount(UserAccount userAccount)
        {
            try
            {
                var repository = new UserAccountRepository();
                repository.CreateUserAccount(userAccount);
            }
            catch (Exception ex)
            {
                //Log Exception
                LogingHelper.LogError("an error occured while creating User accounts", ex);
                throw new BusinessException("an error occured while creating User accounts", ex);
            }
        }
        public UserAccount GetUserAccountById(int id)
        {
            try
            {
                var repository = new UserAccountRepository();
                return repository.GetUserAccountById(id);
            }
            catch (Exception ex)
            {
                //Log Exception
                LogingHelper.LogError("an error occured while getting User accounts by id", ex);
                throw new BusinessException("an error occured while creating User accounts", ex);
            }
        }
    }
}
