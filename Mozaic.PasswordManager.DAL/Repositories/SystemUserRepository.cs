using Microsoft.EntityFrameworkCore;
using Mozaic.PasswordManager.Entities.SearchFilters;
using Mozaic.PasswordManager.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;



namespace Mozaic.PasswordManager.DAL
{
    internal class SystemUserRepository : RepositoryBase
    {
        private readonly string _connectionString;

        public SystemUserRepository()
        {
            _connectionString = AppDbContext.Database.GetDbConnection().ConnectionString;
        }

        public List<SystemUser> GetSystemUser(SystemUserSearchFilter filter)
        {
            return AppDbContext.SystemUsers.ToList();
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
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand createuser = new SqlCommand("dbo.CreateSystemUser", conn))
                {
                    createuser.CommandType = CommandType.StoredProcedure;

                    createuser.Parameters.AddWithValue("@UserName", user.UserName);
                    createuser.Parameters.AddWithValue("@Password", user.password);
                    createuser.Parameters.AddWithValue("@CreatedDate", user.CreationDate);

                    conn.Open();
                    await createuser.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
