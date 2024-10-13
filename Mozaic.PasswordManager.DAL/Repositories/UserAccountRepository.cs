using Microsoft.EntityFrameworkCore;
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

            return list.ToList(); // Ensure this returns all matching accounts
        }

        public void CreateUserAccount(UserAccount userAccount)
        {
            AppDbContext.UserAccounts.Add(userAccount);
            AppDbContext.SaveChanges();
        }

        public UserAccount GetUserAccountById(int id)
        {
            return AppDbContext.UserAccounts.FirstOrDefault(ua => ua.Id == id);
        }
    }
}
