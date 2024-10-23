using Microsoft.EntityFrameworkCore;
using Mozaic.PasswordManager.DAL.Exceptions;
using Mozaic.PasswordManager.Entities;
using Mozaic.PasswordManager.Entities.SearchFilters;
using Mozaic.PasswordManager.Web.Models.DBEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozaic.PasswordManager.DAL
{
    internal class UserAccountRepository:RepositoryBase
    {

        public List<UserAccount> GetUserAccounts(UserAccountSearchFilter filter)
        {
            var list = AppDbContext.UserAccounts.Where(ua => ua.UserId == filter.Id);

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                list = list.Where(ua => ua.AccountName.Contains(filter.Name));
            }

            if (!string.IsNullOrWhiteSpace(filter.UserName))
            {
                list = list.Where(ua => ua.UserName.Contains(filter.UserName));
            }

            try
            {
                return list.ToList();
            }
            catch (Exception ex)
            {

                throw new DataProviderException("DATABASE EXCEPTION WHILE OCCURED WHILE Getting  USER Account", ex);
            }
            
        }

        public void CreateUserAccount(UserAccount userAccount)
        {
            try
            {
                AppDbContext.UserAccounts.Add(userAccount);
                AppDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new DataProviderException("DATABASE EXCEPTION WHILE OCCURED WHILE Creating System USER ", ex);
            }
        }

        public UserAccount GetUserAccountById(int id)
        {
            try
            {
                return AppDbContext.UserAccounts.FirstOrDefault(ua => ua.Id == id);
            }
            catch (Exception ex)
            {

                throw new DataProviderException("DATABASE EXCEPTION WHILE OCCURED WHILE Getting  USER Account by Id", ex);
            }
        }
    }
}
