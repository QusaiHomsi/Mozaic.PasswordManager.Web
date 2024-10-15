using Microsoft.Data.SqlClient;
using Mozaic.PasswordManager.DAL;
using Mozaic.PasswordManager.Entities.SearchFilters;
using Mozaic.PasswordManager.Entities;
using System.Data;

internal static class SystemUserProvider
{
    internal static async Task CreateUser(SystemUser user)
    {
        using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
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

    public static List<SystemUser> GetSystemUser(SystemUserSearchFilter filter)
    {
        var systemUsers = new List<SystemUser>();

        using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
        {
            SqlCommand GetSystemUser = new SqlCommand("GetSystemUsers", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            GetSystemUser.Parameters.AddWithValue("@UserName", string.IsNullOrEmpty(filter.UserName) ? (object)DBNull.Value : filter.UserName);
            GetSystemUser.Parameters.AddWithValue("@UserId", filter.Id.HasValue ? (object)filter.Id.Value : DBNull.Value);
            GetSystemUser.Parameters.AddWithValue("@IsAdmin", filter.IsAdmin.HasValue ? (object)filter.IsAdmin.Value : DBNull.Value);

            conn.Open();
            using (SqlDataReader reader = GetSystemUser.ExecuteReader())
            {
                while (reader.Read())
                {
                    systemUsers.Add(new SystemUser
                    {
                        Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0), 
                        UserName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        IsAdmin = reader.IsDBNull(2) ? (bool?)null : reader.GetBoolean(2)
                    });
                }
            }
        }

        return systemUsers;
    }



}
