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
            SqlCommand cmd = new SqlCommand("GetSystemUsers", conn)
            {
                CommandType = CommandType.StoredProcedure
            };


            conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    systemUsers.Add(new SystemUser
                    {
                        Id = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        IsAdmin = reader.GetBoolean(2)
                    });
                }
            }
        }

        return systemUsers;
    }


}
